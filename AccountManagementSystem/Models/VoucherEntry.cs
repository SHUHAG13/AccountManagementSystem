namespace MiniAccountApi.Models
{
    public class VoucherEntry
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; } = null!;
        public int AccountId { get; set; }
        public ChartOfAccount Account { get; set; } = null!;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string? Narration { get; set; }
    }
}
