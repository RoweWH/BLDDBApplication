using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/parity")]
    [ApiController]
    public class ParityController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public ParityController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }

        [HttpGet("cases")]
        public async Task<IActionResult> GetCases(
            [FromQuery] string? firstEdge,
            [FromQuery] string? secondEdge,
            [FromQuery] string? firstCorner,
            [FromQuery] string? secondCorner,
            [FromQuery] string? twist)
        {
            bool noParams =
                string.IsNullOrWhiteSpace(firstEdge) &&
                string.IsNullOrWhiteSpace(secondEdge) &&
                string.IsNullOrWhiteSpace(firstCorner) &&
                string.IsNullOrWhiteSpace(secondCorner) &&
                string.IsNullOrWhiteSpace(twist);

            if (noParams)
            {
                var cases = await _algorithmData.LoadAll<ParityModel>();

                foreach (var c in cases)
                {
                    if (string.IsNullOrWhiteSpace(c.Twist))
                    {
                        c.Twist = null;
                    }

                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(cases);
            }

            if (
                string.IsNullOrWhiteSpace(firstEdge) ||
                string.IsNullOrWhiteSpace(secondEdge) ||
                string.IsNullOrWhiteSpace(firstCorner) ||
                string.IsNullOrWhiteSpace(secondCorner)
            )
            {
                return BadRequest(new { Message = "Missing required parity case parameters" });
            }

            var parityCase = new ParityModel(
                firstEdge!,
                secondEdge!,
                firstCorner!,
                secondCorner!,
                twist
            );

            if (!InputValidation.IsValidParityRequest(parityCase))
            {
                return BadRequest(new { Message = "Invalid parity case request" });
            }

            parityCase.Algorithms = await _algorithmData.LoadAlgorithms(parityCase);

            if (string.IsNullOrWhiteSpace(parityCase.Twist))
            {
                parityCase.Twist = null;
            }

            return Ok(parityCase);
        }

        [HttpGet("cases/{caseId:int}/algorithms")]
        public async Task<IActionResult> GetAlgorithmsByCaseId(int caseId)
        {
            var algorithms = await _algorithmData.LoadAlgorithmsByCaseId<ParityModel>(caseId);
            return Ok(algorithms);
        }

        [HttpGet("algorithms/{algorithmId:int}")]
        public async Task<IActionResult> GetAlgorithmById(int algorithmId)
        {
            var algorithm = await _algorithmData.LoadAlgorithmById<ParityModel>(algorithmId);

            if (algorithm == null)
            {
                return NotFound(new { Message = "Algorithm not found" });
            }

            return Ok(algorithm);
        }

        [HttpPost("algorithms")]
        public async Task<IActionResult> PostAlgorithm(ParityModel request)
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
                if (!InputValidation.IsValidParityRequest(request))
                {
                    return BadRequest(new { Message = "Invalid parity case request" });
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
        public async Task<ActionResult<bool>> VerifyAlgorithm(ParityModel parityCaseAndAlgorithm)
        {
            if (parityCaseAndAlgorithm == null ||
                parityCaseAndAlgorithm.Id <= 0 ||
                parityCaseAndAlgorithm.Algorithms == null ||
                parityCaseAndAlgorithm.Algorithms.Count == 0 ||
                string.IsNullOrWhiteSpace(parityCaseAndAlgorithm.Algorithms[0].Algorithm))
            {
                return BadRequest(false);
            }

            CaseModel? caseFromDb = await _algorithmData.GetCase(parityCaseAndAlgorithm);

            if (caseFromDb is not ParityModel populatedParityCase)
            {
                return NotFound(false);
            }

            populatedParityCase.Algorithms = parityCaseAndAlgorithm.Algorithms;

            bool valid = await _algorithmData.VerifyAlgorithm(populatedParityCase);

            return Ok(valid);
        }
    }
}