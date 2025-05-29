using AccountManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniAccountApi.Models;

namespace AccountManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Accountant")]
    public class VouchersController : ControllerBase
    {
        private readonly IGenericRepository<Voucher> _repository;

        public VouchersController(IGenericRepository<Voucher> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vouchers = await _repository.GetAllAsync(v => v.Entries, v => v.Entries.Select(e => e.Account));
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
            // include entries because voucher has child entries
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
