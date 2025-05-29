using AccountManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            // ✅ Only include the Entries, deeper includes must be done manually or via custom logic
            var vouchers = await _repository.GetAllAsync(v => v.Entries);
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var voucher = await _repository.GetByIdAsync(id, v => v.Entries, v => v.Entries.Select(e => e.Account));
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Voucher voucher)
        {
            await _repository.AddAsync(voucher);
            var saved = await _repository.SaveChangesAsync();
            return saved ? Ok(voucher) : BadRequest("Failed to create voucher.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Voucher model)
        {
            var existing = await _repository.GetByIdAsync(id, v => v.Entries);
            if (existing == null) return NotFound();

            existing.VoucherType = model.VoucherType;
            existing.VoucherDate = model.VoucherDate;
            existing.ReferenceNo = model.ReferenceNo;

            existing.Entries.Clear();
            foreach (var entry in model.Entries)
            {
                existing.Entries.Add(entry);
            }

            _repository.Update(existing);
            var saved = await _repository.SaveChangesAsync();

            return saved ? Ok(existing) : BadRequest("Update failed.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _repository.Delete(existing);
            var saved = await _repository.SaveChangesAsync();

            return saved ? Ok("Deleted successfully.") : BadRequest("Delete failed.");
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetVouchers(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? voucherType)
        {
            IQueryable<Voucher> query = _repository.GetQueryable();

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

            return Ok(vouchers);
        }
    }
}
