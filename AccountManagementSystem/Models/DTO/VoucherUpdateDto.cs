namespace AccountManagementSystem.Models.DTO
{
    public class VoucherUpdateDto
    {
        public string VoucherType { get; set; } = null!;
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; } = null!;
        public List<VoucherEntryDto> Entries { get; set; } = new();
    }
}
