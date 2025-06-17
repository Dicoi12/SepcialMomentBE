using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingExpenseController : ControllerBase
    {
        private readonly GenericCrudService<WeddingExpense> _service;
        public WeddingExpenseController(GenericCrudService<WeddingExpense> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResult<List<WeddingExpense>>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResult<WeddingExpense>>> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResult<List<WeddingExpense>>>> GetByProperty([FromQuery] string propertyName, [FromQuery] string value)
            => Ok(await _service.GetByPropertyAsync(propertyName, value));

        [HttpPost]
        public async Task<ActionResult<ServiceResult<WeddingExpense>>> Create([FromBody] WeddingExpense entity)
            => Ok(await _service.CreateAsync(entity));

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResult<WeddingExpense>>> Update(Guid id, [FromBody] WeddingExpense entity)
            => Ok(await _service.UpdateAsync(id, entity));

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResult<bool>>> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
} 