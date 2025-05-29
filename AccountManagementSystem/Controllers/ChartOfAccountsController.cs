using AccountManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniAccountApi.Models;

namespace AccountManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Accountant")]
    public class ChartOfAccountsController : ControllerBase
    {
        private readonly IChartOfAccountRepository _repository;

        public ChartOfAccountsController(IChartOfAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _repository.GetRootAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var account = await _repository.GetByIdAsync(id);
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChartOfAccount account)
        {
            await _repository.AddAsync(account);
            return CreatedAtAction(nameof(Get), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChartOfAccount account)
        {
            if (id != account.Id)
                return BadRequest();

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // Update properties
            existing.AccountName = account.AccountName;
            existing.ParentId = account.ParentId;

            await _repository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
