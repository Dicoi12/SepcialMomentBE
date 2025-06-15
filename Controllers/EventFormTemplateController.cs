using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventFormTemplateController : ControllerBase
    {
        private readonly GenericCrudService<EventFormTemplate> _service;
        public EventFormTemplateController(GenericCrudService<EventFormTemplate> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<EventFormTemplate>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<EventFormTemplate>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpPost]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> Create([FromBody] EventFormTemplate entity)
            => Ok(await _service.CreateAsync(entity));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<EventFormTemplate>>> Update(Guid id, [FromBody] EventFormTemplate entity)
            => Ok(await _service.UpdateAsync(id, entity));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 