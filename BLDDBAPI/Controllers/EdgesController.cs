using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet("buffer={buffer}&first={first}&second={second}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
            CycleModel edgeCase = new CycleModel(buffer, first, second);
            if (InputValidation.IsValidEdgeRequest(edgeCase))
            {
                var algorithms = await _algorithmData.LoadEdgeAlgorithms(edgeCase);
                return Ok(algorithms);
            }
            else return BadRequest(new { Message = "Invalid edge case request" });
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<List<IActionResult>> PostAlgorithms(List<AlgorithmModel> newAlgorithms)
        {
            List<IActionResult> results = new List<IActionResult>();
            foreach (var alg in newAlgorithms)
            {
                int id = await _algorithmData.InsertEdgeAlg(alg);
                if (id != 0)
                {
                    results.Add(Ok(new { Id = id }));
                }
                else
                    results.Add(BadRequest(new { Message = "Invalid Algorithm" }));
            }
            return results;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase([FromQuery] CycleModel edgeCase, [FromBody] AlgorithmModel newAlgorithm)
        {
            if (InputValidation.IsValidEdgeRequest(edgeCase))
            {
                int id = await _algorithmData.InsertEdgeAlgByCase(edgeCase, newAlgorithm);
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


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _algorithmData.DeleteEdgeAlg(id);
            if (id != 0)
            {
                return Ok();
            }
            else
                return NotFound();
        }
    }
}

