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
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
            EdgeCycleModel edgeCase = new EdgeCycleModel(buffer, first, second);
            if (InputValidation.IsValidEdgeRequest(edgeCase))
            {
                var algorithms = await _algorithmData.LoadAlgorithms(edgeCase);
                return Ok(algorithms);
            }
            else return BadRequest(new { Message = "Invalid edge case request" });
        }
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAlgorithms(List<AlgorithmModel> newAlgorithms)
        {
            List<string> validAlgorithms = new List<string>();
            List<string> invalidAlgorithms = new List<string>();
            foreach (var alg in newAlgorithms)
            {

                int id = await _algorithmData.InsertAlg(alg);
                if (id != 0)
                {
                    validAlgorithms.Add(alg.Algorithm);
                }
                else
                {
                    invalidAlgorithms.Add(alg.Algorithm);
                }
            }
            return Ok(new
            {
                ValidAlgorithms = validAlgorithms,
                InvalidAlgorithms = invalidAlgorithms
            });
        }
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase([FromQuery] EdgeCycleModel edgeCase, [FromBody] AlgorithmModel newAlgorithm)
        {
            if (InputValidation.IsValidEdgeRequest(edgeCase))
            {
                newAlgorithm.Case = edgeCase;
                int id = await _algorithmData.InsertAlgByCase(newAlgorithm);
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

        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="id">Id of algorithm to be deleted</param>
        /// <returns>Ok or NotFound</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(AlgorithmModel algorithm)
        {
            int id = await _algorithmData.DeleteAlg(algorithm);
            if (id != 0)
            {
                return Ok();
            }
            else
                return NotFound();
        }
    }
}

