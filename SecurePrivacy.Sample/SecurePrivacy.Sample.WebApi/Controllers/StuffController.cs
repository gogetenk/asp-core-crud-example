using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurePrivacy.Sample.Dto;

namespace SecurePrivacy.Sample.WebApi.Controllers
{
    [ApiController]
    [Route("api/stuff")]
    public class StuffController : ControllerBase
    {
        private readonly ILogger<StuffController> _logger;

        public StuffController(ILogger<StuffController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<StuffDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("GET operation started.");
            return Ok(new List<StuffDto>());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StuffDto))]
        public async Task<IActionResult> Post(StuffDto stuff)
        {
            _logger.LogInformation("GET operation started.");
            return Created("", new StuffDto());
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(string id, StuffDto stuff)
        {
            _logger.LogInformation("GET operation started.");
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("GET operation started.");
            return NoContent();
        }
    }
}
