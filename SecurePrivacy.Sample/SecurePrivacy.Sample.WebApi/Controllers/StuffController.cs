using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecurePrivacy.Sample.Bll.Services;
using SecurePrivacy.Sample.Dto;
using SecurePrivacy.Sample.Model;

namespace SecurePrivacy.Sample.WebApi.Controllers
{
    [ApiController]
    [Route("api/stuff")]
    public class StuffController : ControllerBase
    {
        private readonly ILogger<StuffController> _logger;
        private readonly IMapper _mapper;
        private readonly IStuffService _stuffService;

        public StuffController(ILogger<StuffController> logger, IMapper mapper, IStuffService stuffService)
        {
            _logger = logger;
            _mapper = mapper;
            _stuffService = stuffService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(StuffDto))]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation("GET operation started.");
            var result = await _stuffService.GetAsync(id);
            return Ok(_mapper.Map<StuffDto>(result));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StuffDto))]
        public async Task<IActionResult> Post(StuffDto stuff)
        {
            _logger.LogInformation("POST operation started.");
            var result = await _stuffService.CreateAsync(_mapper.Map<Stuff>(stuff));
            return Created("", _mapper.Map<StuffDto>(result));
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(string id, StuffDto stuff)
        {
            _logger.LogInformation("PUT operation started.");
            if (await _stuffService.UpdateAsync(id, _mapper.Map<Stuff>(stuff)))
                return NoContent();
            else
                return BadRequest("The entity doesn't exist.");
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("DELETE operation started.");
            await _stuffService.DeleteAsync(id);
            return NoContent();
        }
    }
}
