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

        [HttpGet("buffer={buffer}&first={first}&second={second}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
            CycleModel cornerCase = new CycleModel(buffer, first, second);
            if (InputValidation.IsValidCornerRequest(cornerCase))
            {
                var algorithms = await _algorithmData.LoadCornerAlgorithms(cornerCase);
                return Ok(algorithms);
            }
            else return BadRequest(new { Message = "Invalid corner case request" });
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
                int id = await _algorithmData.InsertCornerAlg(alg);
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
        public async Task<IActionResult> PostAlgorithmByCase([FromQuery] CycleModel cornerCase, [FromBody] AlgorithmModel newAlgorithm)
        {
            if (InputValidation.IsValidCornerRequest(cornerCase))
            {
                int id = await _algorithmData.InsertCornerAlgByCase(cornerCase, newAlgorithm);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _algorithmData.DeleteCornerAlg(id);
            if (id != 0)
            {
                return Ok();
            }
            else
                return NotFound();
        }
    }
}
