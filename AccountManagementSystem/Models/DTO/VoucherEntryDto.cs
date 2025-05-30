namespace AccountManagementSystem.Models.DTO
{
    public class VoucherEntryDto
    {
        public int AccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; } = null!;
    }
}
