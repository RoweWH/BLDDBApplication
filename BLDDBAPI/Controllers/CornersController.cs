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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
        [FromQuery] string? buffer,
        [FromQuery] string? first,
        [FromQuery] string? second)
        {
            bool hasBuffer = !string.IsNullOrWhiteSpace(buffer);
            bool hasFirst = !string.IsNullOrWhiteSpace(first);
            bool hasSecond = !string.IsNullOrWhiteSpace(second);

            bool noParams = !hasBuffer && !hasFirst && !hasSecond;
            bool bufferOnly = hasBuffer && !hasFirst && !hasSecond;
            bool allParams = hasBuffer && hasFirst && hasSecond;

            if (noParams)
            {
                var cornerCases = await _algorithmData.LoadAll<CornerCycleModel>();

                foreach (var c in cornerCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms<CornerCycleModel>(c);
                }

                return Ok(cornerCases);
            }

            if (bufferOnly)
            {
                if (!InputValidation.IsValidCorner(buffer!))
                {
                    return BadRequest(new { Message = "Invalid buffer" });
                }

                var cornerCases = await _algorithmData.LoadCasesByBuffer<CornerCycleModel>(buffer!);

                foreach (var c in cornerCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(cornerCases);
            }

            if (allParams)
            {
                var cornerCase = new CornerCycleModel(buffer!, first!, second!);

                if (!InputValidation.IsValidCornerRequest(cornerCase))
                {
                    return BadRequest(new { Message = "Invalid corner case request" });
                }

                cornerCase.Algorithms = await _algorithmData.LoadAlgorithms<CornerCycleModel>(cornerCase);

                return Ok(cornerCase);
            }

            return BadRequest(new
            {
                Message = "Invalid query. Use no parameters, buffer only, or buffer + first + second."
            });
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CornerCycleModel cornerCaseAndAlgorithm)
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
