using AccountManagementSystem.Interfaces;
using AccountManagementSystem.Models;
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
            var response = await _repository.GetAllAsync(c => c.Children, c => c.Parent);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _repository.GetByIdAsync(id, c => c.Children, c => c.Parent);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChartOfAccount account)
        {
            var response = await _repository.AddAsync(account);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChartOfAccount model)
        {
            var getResponse = await _repository.GetByIdAsync(id);
            if (!getResponse.IsSuccess || getResponse.Data is not ChartOfAccount existing)
                return NotFound(getResponse);

            existing.AccountName = model.AccountName;
            existing.ParentId = model.ParentId;

            var updateResponse = await _repository.UpdateAsync(existing);
            return updateResponse.IsSuccess ? Ok(updateResponse) : BadRequest(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var getResponse = await _repository.GetByIdAsync(id);
            if (!getResponse.IsSuccess || getResponse.Data is not ChartOfAccount existing)
                return NotFound(getResponse);

            var deleteResponse = await _repository.DeleteAsync(existing);
            return deleteResponse.IsSuccess ? Ok(deleteResponse) : BadRequest(deleteResponse);
        }
    }
}
