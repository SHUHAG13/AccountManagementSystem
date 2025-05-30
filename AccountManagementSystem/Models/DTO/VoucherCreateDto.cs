namespace AccountManagementSystem.Models.DTO
{
    public class VoucherCreateDto
    {
        public string VoucherType { get; set; } = null!;
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; } = null!;
        public List<VoucherEntryCreateDto> Entries { get; set; } = new();
    }
}
