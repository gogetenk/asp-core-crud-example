using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurePrivacy.Sample.Dto;

namespace SecurePrivacy.Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StuffController : ControllerBase
    {
        private readonly ILogger<StuffController> _logger;

        public StuffController(ILogger<StuffController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("GET operation started.");
            return Ok(new List<Stuff>());
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            _logger.LogInformation("GET operation started.");
            return Created("", new Stuff());
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            _logger.LogInformation("GET operation started.");
            return Ok(new Stuff());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            _logger.LogInformation("GET operation started.");
            return Ok();
        }
    }
}
