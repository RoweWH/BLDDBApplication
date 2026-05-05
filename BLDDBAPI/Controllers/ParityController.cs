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

        /*[HttpGet("edge1={edge1}&edge2={edge2}&corner1={corner1}&corner2={corner2}&twist={twist}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string edge1, string edge2, string corner1, string corner2, string? twist)
        {
            ParityModel parityCase = new ParityModel(edge1, edge2, corner1, corner2, twist);
            if (InputValidation.IsValidParityRequest(parityCase))
            {
                parityCase.Algorithms = await _algorithmData.LoadAlgorithms(parityCase);
                return Ok(parityCase);
            }
            else return BadRequest(new { Message = "Invalid parity case request" });
        }
        */

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
        [FromQuery] string firstEdge,
        [FromQuery] string secondEdge,
        [FromQuery] string firstCorner,
        [FromQuery] string secondCorner,
        [FromQuery] string? twist)
        {
            ParityModel parityCase = new ParityModel(firstEdge, secondEdge, firstCorner, secondCorner, twist);

            if (InputValidation.IsValidParityRequest(parityCase))
            {
                parityCase.Algorithms = await _algorithmData.LoadAlgorithms(parityCase);
                return Ok(parityCase);
            }

            return BadRequest(new { Message = "Invalid parity case request" });
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase(ParityModel parityCaseAndAlgorithm)
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