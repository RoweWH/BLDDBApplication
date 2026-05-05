using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdgesController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public EdgesController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery]string buffer, [FromQuery] string? first, [FromQuery] string? second)
        {
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            {
                if (InputValidation.IsValidEdge(buffer))
                {
                   var edgeCases = await _algorithmData.LoadCasesByBuffer(buffer);

                    foreach (var c in edgeCases)
                    {
                        c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                    }
                    return Ok(edgeCases);
                }
                else return BadRequest(new { Message = "Invalid edge case request" });
            }
            EdgeCycleModel edgeCase = new EdgeCycleModel(buffer, first, second);
            if (InputValidation.IsValidEdgeRequest(edgeCase))
            {
                edgeCase.Algorithms = await _algorithmData.LoadAlgorithms(edgeCase);
                return Ok(edgeCase);
            }
            else return BadRequest(new { Message = "Invalid edge case request" });
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase(EdgeCycleModel edgeCaseAndAlgorithm)
        {
            if (InputValidation.IsValidEdgeRequest(edgeCaseAndAlgorithm))
            {
                int id = await _algorithmData.InsertAlgByCase(edgeCaseAndAlgorithm);
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
                return BadRequest(new { Message = "Invalid edge case request" });
            }
        }
    }
}