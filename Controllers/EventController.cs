using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var event_ = await _eventService.GetEventByIdAsync(id);
            if (event_ == null)
            {
                return NotFound();
            }
            return Ok(event_);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event event_)
        {
            var createdEvent = await _eventService.CreateEventAsync(event_);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPost("with-form")]
        public async Task<ActionResult<Event>> CreateEventWithForm([FromBody] CreateEventWithFormRequest request)
        {
            try
            {
                // Extrag UserId din token-ul de autentificare
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("User ID invalid din token");
                }

                // Creez evenimentul
                var event_ = new Event
                {
                    UserId = userId,
                    Title = request.Title,
                    Description = request.Description,
                    Date = request.Date,
                    Location = request.Location,
                    EventType = request.EventType,
                    CreatedAt = DateTime.UtcNow
                };

                // Convertesc datele formularului în EventForm
                var eventForms = request.FormData.Select(fd => new EventForm
                {
                    FormTemplateId = fd.FormTemplateId,
                    FieldValue = fd.FieldValue
                }).ToList();

                var createdEvent = await _eventService.CreateEventWithFormAsync(event_, eventForms);
                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Eroare la crearea evenimentului: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event event_)
        {
            if (id != event_.Id)
            {
                return BadRequest();
            }

            var result = await _eventService.UpdateEventAsync(event_);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 