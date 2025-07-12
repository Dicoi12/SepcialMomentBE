using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventFormProviderController : ControllerBase
    {
        private readonly EventFormTemplateService _eventFormTemplateService;

        public EventFormProviderController(EventFormTemplateService eventFormTemplateService)
        {
            _eventFormTemplateService = eventFormTemplateService;
        }

        [HttpGet("by-event-type/{eventType}")]
        public async Task<IActionResult> GetTemplatesByEventType([FromQuery]EventType eventType)
        {
            var templates = await _eventFormTemplateService.GetTemplatesByEventTypeAsync(eventType);
            return Ok(templates);
        }
    }
}