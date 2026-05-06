using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParityController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public ParityController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
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
                foreach(var c in cases)
                {
                    if (string.IsNullOrWhiteSpace(c.Twist)) c.Twist = null;
                    c.Algorithms = await _algorithmData.LoadAlgorithms<ParityModel>(c);
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

            ParityModel parityCase = new ParityModel(
                firstEdge,
                secondEdge,
                firstCorner,
                secondCorner,
                twist
            );

            if (InputValidation.IsValidParityRequest(parityCase))
            {
                parityCase.Algorithms = await _algorithmData.LoadAlgorithms<ParityModel>(parityCase);
                return Ok(parityCase);
            }

            return BadRequest(new { Message = "Invalid parity case request" });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(ParityModel parityCaseAndAlgorithm)
        {
            if (InputValidation.IsValidParityRequest(parityCaseAndAlgorithm))
            {
                int id = await _algorithmData.InsertAlgByCase(parityCaseAndAlgorithm);
                if (id > 0)
                {
                    return Ok(new { Id = id });
                }
                else if (id == -1)
                {
                    return BadRequest(new { Message = "Algorithm already exists" });
                }
                else
                    return BadRequest(new { Message = "Invalid Algorithm" });
            }
            else
            {
                return BadRequest(new { Message = "Invalid parity case request" });
            }
        }
    }
}