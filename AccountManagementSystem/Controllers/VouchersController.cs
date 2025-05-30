using AccountManagementSystem.Interfaces;
using AccountManagementSystem.Models;
using AccountManagementSystem.Models.DTO;
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
            var response = await _repository.GetQueryable().Include(v => v.Entries)
                .ThenInclude(e => e.Account)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (response == null)
                return NotFound(new ResponseModel { IsSuccess = false, Message = "Voucher not found." });
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VoucherCreateDto dto)
        {
            var voucher = new Voucher
            {
                VoucherType = dto.VoucherType,
                VoucherDate = dto.VoucherDate,
                ReferenceNo = dto.ReferenceNo,
                Entries = dto.Entries.Select(e => new VoucherEntry
                {
                    AccountId = e.AccountId,
                    DebitAmount = e.DebitAmount,
                    CreditAmount = e.CreditAmount,
                    Narration = e.Narration
                }).ToList()
            };

            var response = await _repository.AddAsync(voucher);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
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
