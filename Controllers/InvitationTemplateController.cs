using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationTemplateController : ControllerBase
    {
        private readonly GenericCrudService<InvitationTemplate> _service;
        public InvitationTemplateController(GenericCrudService<InvitationTemplate> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<InvitationTemplate>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<InvitationTemplate>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<InvitationTemplate>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpPost]
        public async Task<ActionResult<ServiceResult<InvitationTemplate>>> Create([FromBody] InvitationTemplate entity)
            => Ok(await _service.CreateAsync(entity));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<InvitationTemplate>>> Update(Guid id, [FromBody] InvitationTemplate entity)
            => Ok(await _service.UpdateAsync(id, entity));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 