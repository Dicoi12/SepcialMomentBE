using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventFormController : ControllerBase
    {
        private readonly GenericCrudService<EventForm> _service;
        public EventFormController(GenericCrudService<EventForm> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<EventForm>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<EventForm>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<EventForm>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpPost]
        public async Task<ActionResult<ServiceResult<EventForm>>> Create([FromBody] EventForm entity)
            => Ok(await _service.CreateAsync(entity));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<EventForm>>> Update(Guid id, [FromBody] EventForm entity)
            => Ok(await _service.UpdateAsync(id, entity));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 