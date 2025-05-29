using AccountManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniAccountApi.Models;

namespace AccountManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class ChartOfAccountsController : ControllerBase
    {
        private readonly IGenericRepository<ChartOfAccount> _repository;

        public ChartOfAccountsController(IGenericRepository<ChartOfAccount> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _repository.GetAllAsync(c => c.Children, c => c.Parent);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var account = await _repository.GetByIdAsync(id, c => c.Children, c => c.Parent);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChartOfAccount account)
        {
            await _repository.AddAsync(account);
            var saved = await _repository.SaveChangesAsync();
            return saved ? Ok(account) : BadRequest("Create failed");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChartOfAccount model)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.AccountName = model.AccountName;
            existing.ParentId = model.ParentId;

            _repository.Update(existing);
            var saved = await _repository.SaveChangesAsync();

            return saved ? Ok(existing) : BadRequest("Update failed");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _repository.Delete(existing);
            var saved = await _repository.SaveChangesAsync();

            return saved ? Ok("Deleted successfully") : BadRequest("Delete failed");
        }

    }
}