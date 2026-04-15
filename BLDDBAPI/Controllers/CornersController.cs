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
        
        [HttpGet("corner1={buffer}&corner2={first}&corner3={second}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string buffer, string first, string second)
        {
            CornerCycleModel cornerCase = new CornerCycleModel(buffer, first, second);
            if (InputValidation.IsValidCornerRequest(cornerCase))
            {
                cornerCase.Algorithms = await _algorithmData.LoadAlgorithms(cornerCase);
                return Ok(cornerCase);
            }
            else return BadRequest(new { Message = "Invalid corner case request" });
        }
        

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAlgorithmByCase(CornerCycleModel cornerCaseAndAlgorithm)
        {
            if (InputValidation.IsValidCornerRequest(cornerCaseAndAlgorithm))
            {
                int id = await _algorithmData.InsertAlgByCase(cornerCaseAndAlgorithm);
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
                return BadRequest(new { Message = "Invalid corner case request" });
            }
        }
        
    }
}
