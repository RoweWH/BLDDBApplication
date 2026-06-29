using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/corners")]
    [ApiController]
    public class CornersController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public CornersController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }

        [HttpGet("cases")]
        public async Task<IActionResult> GetCases(
            [FromQuery] string? buffer,
            [FromQuery] string? first,
            [FromQuery] string? second)
        {
            bool hasBuffer = !string.IsNullOrWhiteSpace(buffer);
            bool hasFirst = !string.IsNullOrWhiteSpace(first);
            bool hasSecond = !string.IsNullOrWhiteSpace(second);

            bool noParams = !hasBuffer && !hasFirst && !hasSecond;
            bool bufferOnly = hasBuffer && !hasFirst && !hasSecond;
            bool allParams = hasBuffer && hasFirst && hasSecond;

            if (noParams)
            {
                var cornerCases = await _algorithmData.LoadAll<CornerCycleModel>();

                foreach (var c in cornerCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(cornerCases);
            }

            if (bufferOnly)
            {
                if (!InputValidation.IsValidCorner(buffer!))
                {
                    return BadRequest(new { Message = "Invalid buffer" });
                }

                var cornerCases = await _algorithmData.LoadCasesByBuffer<CornerCycleModel>(buffer!);

                foreach (var c in cornerCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(cornerCases);
            }

            if (allParams)
            {
                var cornerCase = new CornerCycleModel(buffer!, first!, second!);

                if (!InputValidation.IsValidCornerRequest(cornerCase))
                {
                    return BadRequest(new { Message = "Invalid corner case request" });
                }

                cornerCase.Algorithms = await _algorithmData.LoadAlgorithms(cornerCase);

                return Ok(cornerCase);
            }

            return BadRequest(new
            {
                Message = "Invalid query. Use no parameters, buffer only, or buffer + first + second."
            });
        }

        [HttpGet("cases/{caseId:int}/algorithms")]
        public async Task<IActionResult> GetAlgorithmsByCaseId(int caseId)
        {
            var algorithms = await _algorithmData.LoadAlgorithmsByCaseId<CornerCycleModel>(caseId);
            return Ok(algorithms);
        }

        [HttpGet("algorithms/{algorithmId:int}")]
        public async Task<IActionResult> GetAlgorithmById(int algorithmId)
        {
            var algorithm = await _algorithmData.LoadAlgorithmById<CornerCycleModel>(algorithmId);

            if (algorithm == null)
            {
                return NotFound(new { Message = "Algorithm not found" });
            }

            return Ok(algorithm);
        }

        [HttpPost("algorithms")]
        public async Task<IActionResult> PostAlgorithm(CornerCycleModel request)
        {
            if (request == null ||
                request.Algorithms == null ||
                request.Algorithms.Count == 0 ||
                string.IsNullOrWhiteSpace(request.Algorithms[0].Algorithm))
            {
                return BadRequest(new { Message = "Algorithm is required" });
            }

            CaseModel? caseToUse = null;

            if (request.Id > 0)
            {
                caseToUse = await _algorithmData.GetCase(request);

                if (caseToUse == null)
                {
                    return NotFound(new { Message = "Case not found" });
                }

                caseToUse.Algorithms = request.Algorithms;
            }
            else
            {
                if (!InputValidation.IsValidCornerRequest(request))
                {
                    return BadRequest(new { Message = "Invalid corner case request" });
                }

                caseToUse = request;
            }

            int id = await _algorithmData.InsertAlgByCase(caseToUse);

            if (id > 0)
            {
                return Ok(new { Id = id });
            }

            if (id == -1)
            {
                return BadRequest(new { Message = "Algorithm already exists" });
            }

            return BadRequest(new { Message = "Invalid Algorithm" });
        }

        [HttpPost("algorithms/verify")]
        public async Task<ActionResult<bool>> VerifyAlgorithm(CornerCycleModel cornerCaseAndAlgorithm)
        {
            if (cornerCaseAndAlgorithm == null ||
                cornerCaseAndAlgorithm.Id <= 0 ||
                cornerCaseAndAlgorithm.Algorithms == null ||
                cornerCaseAndAlgorithm.Algorithms.Count == 0 ||
                string.IsNullOrWhiteSpace(cornerCaseAndAlgorithm.Algorithms[0].Algorithm))
            {
                return BadRequest(false);
            }

            CaseModel? caseFromDb = await _algorithmData.GetCase(cornerCaseAndAlgorithm);

            if (caseFromDb is not CornerCycleModel populatedCornerCase)
            {
                return NotFound(false);
            }

            populatedCornerCase.Algorithms = cornerCaseAndAlgorithm.Algorithms;

            bool valid = await _algorithmData.VerifyAlgorithm(populatedCornerCase);

            return Ok(valid);
        }
    }
}