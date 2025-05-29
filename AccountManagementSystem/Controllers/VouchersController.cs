using AccountManagementSystem.Interfaces;
using AccountManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniAccountApi.Models;

namespace AccountManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IGenericRepository<Voucher> _repository;

        public VouchersController(IGenericRepository<Voucher> repository)
        {
            _repository = repository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _repository.GetAllAsync(v => v.Entries);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _repository.GetByIdAsync(id, v => v.Entries, v => v.Entries.Select(e => e.Account));
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Voucher voucher)
        {
            var response = await _repository.AddAsync(voucher);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Voucher model)
        {
            var getResponse = await _repository.GetByIdAsync(id, v => v.Entries);
            if (!getResponse.IsSuccess || getResponse.Data is not Voucher existing)
                return NotFound(getResponse);

            existing.VoucherType = model.VoucherType;
            existing.VoucherDate = model.VoucherDate;
            existing.ReferenceNo = model.ReferenceNo;

            existing.Entries.Clear();
            foreach (var entry in model.Entries)
                existing.Entries.Add(entry);

            var updateResponse = await _repository.UpdateAsync(existing);
            return updateResponse.IsSuccess ? Ok(updateResponse) : BadRequest(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var getResponse = await _repository.GetByIdAsync(id);
            if (!getResponse.IsSuccess || getResponse.Data is not Voucher existing)
                return NotFound(getResponse);

            var deleteResponse = await _repository.DeleteAsync(existing);
            return deleteResponse.IsSuccess ? Ok(deleteResponse) : BadRequest(deleteResponse);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? voucherType)
        {
            var query = _repository.GetQueryable();

            if (fromDate.HasValue)
                query = query.Where(v => v.VoucherDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(v => v.VoucherDate <= toDate.Value);
            if (!string.IsNullOrEmpty(voucherType))
                query = query.Where(v => v.VoucherType == voucherType);

            var vouchers = await query
                .Include(v => v.Entries)
                .ThenInclude(e => e.Account)
                .ToListAsync();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Filtered vouchers retrieved successfully.",
                Data = vouchers
            });
        }
    }
}
