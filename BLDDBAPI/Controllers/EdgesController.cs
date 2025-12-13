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

        [HttpGet("edge1={buffer}&edge2={first}&edge3={second}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
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
                if (id != 0)
                {
                    return Ok(new { Id = id });
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