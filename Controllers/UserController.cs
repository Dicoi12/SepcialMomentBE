using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GenericCrudService<User> _service;
        public UserController(GenericCrudService<User> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<User>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<User>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<User>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpPost]
        public async Task<ActionResult<ServiceResult<User>>> Create([FromBody] User user)
            => Ok(await _service.CreateAsync(user));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<User>>> Update(Guid id, [FromBody] User user)
            => Ok(await _service.UpdateAsync(id, user));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 