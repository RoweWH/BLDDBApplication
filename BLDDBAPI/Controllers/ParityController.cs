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

        [HttpGet("edge1={firstEdge}&edge2={secondEdge}&corner1={firstCorner}&corner2={secondCorner}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string firstEdge, string secondEdge, string firstCorner, string secondCorner, string? twist)
        {
            ParityModel parityCase = new ParityModel(firstEdge, secondEdge, firstCorner, secondCorner, twist);
            if (InputValidation.IsValidParityRequest(parityCase))
            {
                parityCase.Algorithms = await _algorithmData.LoadAlgorithms(parityCase);
                return Ok(parityCase);
            }
            else return BadRequest(new { Message = "Invalid parity case request" });
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
                if (id != 0)
                {
                    return Ok(new { Id = id });
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