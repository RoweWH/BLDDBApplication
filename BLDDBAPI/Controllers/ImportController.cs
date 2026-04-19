using BLDAPI.Validation;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IAlgorithmData _algorithmData;

        public ImportController(IAlgorithmData algorithmData)
        {
            _algorithmData = algorithmData;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportAlgorithms(List<string> newAlgorithms)
        {
            List<AlgorithmModel> validAlgorithms = new List<AlgorithmModel>();
            List<string> invalidAlgorithms = new List<string>();
            List<string> duplicateAlgorithms = new List<string>();
            foreach (var alg in newAlgorithms)
            {
                int id = await _algorithmData.InsertAlg(alg);
                if(id == -1)
                {
                    duplicateAlgorithms.Add(alg);
                }
                else if(id == 0)
                {
                    invalidAlgorithms.Add(alg);
                }
                else
                {
                    validAlgorithms.Add(new AlgorithmModel { Id = id, Algorithm = alg });
                }
                
            }
            return Ok(new
            {
                ValidAlgorithms = validAlgorithms,
                DuplicateAlgorithms = duplicateAlgorithms,
                InvalidAlgorithms = invalidAlgorithms
            });
        }
    }
}

