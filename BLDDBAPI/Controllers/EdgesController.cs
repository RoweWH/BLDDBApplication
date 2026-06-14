using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/edges")]
    [ApiController]
    public class EdgesController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public EdgesController(IAlgorithmData algorithmData)
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
                var edgeCases = await _algorithmData.LoadAll<EdgeCycleModel>();

                foreach (var c in edgeCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(edgeCases);
            }

            if (bufferOnly)
            {
                if (!InputValidation.IsValidEdge(buffer!))
                {
                    return BadRequest(new { Message = "Invalid buffer" });
                }

                var edgeCases = await _algorithmData.LoadCasesByBuffer<EdgeCycleModel>(buffer!);

                foreach (var c in edgeCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(edgeCases);
            }

            if (allParams)
            {
                var edgeCase = new EdgeCycleModel(buffer!, first!, second!);

                if (!InputValidation.IsValidEdgeRequest(edgeCase))
                {
                    return BadRequest(new { Message = "Invalid edge case request" });
                }

                edgeCase.Algorithms = await _algorithmData.LoadAlgorithms(edgeCase);

                return Ok(edgeCase);
            }

            return BadRequest(new
            {
                Message = "Invalid query. Use no parameters, buffer only, or buffer + first + second."
            });
        }

        [HttpGet("cases/{caseId:int}/algorithms")]
        public async Task<IActionResult> GetAlgorithmsByCaseId(int caseId)
        {
            var algorithms = await _algorithmData.LoadAlgorithmsByCaseId<EdgeCycleModel>(caseId);
            return Ok(algorithms);
        }

        [HttpGet("algorithms/{algorithmId:int}")]
        public async Task<IActionResult> GetAlgorithmById(int algorithmId)
        {
            var algorithm = await _algorithmData.LoadAlgorithmById<EdgeCycleModel>(algorithmId);

            if (algorithm == null)
            {
                return NotFound(new { Message = "Algorithm not found" });
            }

            return Ok(algorithm);
        }

        [HttpPost("algorithms")]
        public async Task<IActionResult> PostAlgorithm(EdgeCycleModel edgeCaseAndAlgorithm)
        {
            if (!InputValidation.IsValidEdgeRequest(edgeCaseAndAlgorithm))
            {
                return BadRequest(new { Message = "Invalid edge case request" });
            }

            int id = await _algorithmData.InsertAlgByCase(edgeCaseAndAlgorithm);

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
        public async Task<ActionResult<bool>> VerifyAlgorithm(EdgeCycleModel edgeCaseAndAlgorithm)
        {
            if (edgeCaseAndAlgorithm == null ||
                edgeCaseAndAlgorithm.Id <= 0 ||
                edgeCaseAndAlgorithm.Algorithms == null ||
                edgeCaseAndAlgorithm.Algorithms.Count == 0 ||
                string.IsNullOrWhiteSpace(edgeCaseAndAlgorithm.Algorithms[0].Algorithm))
            {
                return BadRequest(false);
            }

            CaseModel? caseFromDb = await _algorithmData.GetCase(edgeCaseAndAlgorithm);

            if (caseFromDb is not EdgeCycleModel populatedEdgeCase)
            {
                return NotFound(false);
            }

            populatedEdgeCase.Algorithms = edgeCaseAndAlgorithm.Algorithms;

            bool valid = await _algorithmData.VerifyAlgorithm(populatedEdgeCase);

            return Ok(valid);
        }


    }
}