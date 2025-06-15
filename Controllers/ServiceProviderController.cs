using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceProviderController : ControllerBase
    {
        private readonly IWeddingService _weddingService;

        public ServiceProviderController(IWeddingService weddingService)
        {
            _weddingService = weddingService;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<WeddingServiceProvider>>> GetServiceProviders(int eventId)
        {
            var providers = await _weddingService.GetServiceProvidersAsync(eventId);
            return Ok(providers);
        }

        [HttpPost]
        public async Task<ActionResult<WeddingServiceProvider>> CreateServiceProvider(WeddingServiceProvider provider)
        {
            var createdProvider = await _weddingService.CreateServiceProviderAsync(provider);
            return CreatedAtAction(nameof(GetServiceProviders), new { eventId = createdProvider.EventId }, createdProvider);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceProvider(int id, WeddingServiceProvider provider)
        {
            if (id != provider.Id)
            {
                return BadRequest();
            }

            var result = await _weddingService.UpdateServiceProviderAsync(provider);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceProvider(int id)
        {
            var result = await _weddingService.DeleteServiceProviderAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 