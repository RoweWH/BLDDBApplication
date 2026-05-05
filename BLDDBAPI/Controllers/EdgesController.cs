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
                var edgeCases = await _algorithmData.LoadAll<EdgeCycleModel>();

                foreach (var c in edgeCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms(c);
                }

                return Ok(edgeCases);
            }

            if (bufferOnly)
            {
                if (!InputValidation.IsValidEdge(buffer!))
                {
                    return BadRequest(new { Message = "Invalid buffer" });
                }

                var edgeCases = await _algorithmData.LoadCasesByBuffer<EdgeCycleModel>(buffer!);

                foreach (var c in edgeCases)
                {
                    c.Algorithms = await _algorithmData.LoadAlgorithms<EdgeCycleModel>(c);
                }

                return Ok(edgeCases);
            }

            if (allParams)
            {
                var edgeCase = new EdgeCycleModel(buffer!, first!, second!);

                if (!InputValidation.IsValidEdgeRequest(edgeCase))
                {
                    return BadRequest(new { Message = "Invalid edge case request" });
                }

                edgeCase.Algorithms = await _algorithmData.LoadAlgorithms<EdgeCycleModel>(edgeCase);

                return Ok(edgeCase);
            }

            return BadRequest(new
            {
                Message = "Invalid query. Use no parameters, buffer only, or buffer + first + second."
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(EdgeCycleModel edgeCaseAndAlgorithm)
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