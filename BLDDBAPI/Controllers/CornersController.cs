using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CornersController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public CornersController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }
        /// <summary>
        /// Get method for algorithms
        /// </summary>
        /// <param name="buffer">Buffer Piece</param>
        /// <param name="first">First Target</param>
        /// <param name="second">Second Target</param>
        /// <returns>List of algorithms for specific case</returns>
        [HttpGet("buffer={buffer}&first={first}&second={second}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
            CornerCycleModel cornerCase = new CornerCycleModel(buffer, first, second);
            if (InputValidation.IsValidCornerRequest(cornerCase))
            {
                var algorithms = await _algorithmData.LoadAlgorithms(cornerCase);
                return Ok(algorithms);
            }
            else return BadRequest(new { Message = "Invalid corner case request" });
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
            return Ok(new { ValidAlgorithms = validAlgorithms,
                            InvalidAlgorithms = invalidAlgorithms});
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase([FromQuery] CornerCycleModel cornerCase, [FromBody] AlgorithmModel newAlgorithm)
        {
            if (InputValidation.IsValidCornerRequest(cornerCase))
            {
                newAlgorithm.Case = cornerCase;
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
                return BadRequest(new { Message = "Invalid corner case request" });
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] AlgorithmModel algorithm)
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
