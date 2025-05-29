namespace MiniAccountApi.Models
{
    public class VoucherEntry
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public ChartOfAccount? Account { get; set; }  // ✅ Nullable

        public int VoucherId { get; set; }
        public Voucher? Voucher { get; set; }         // ✅ Nullable

        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; } = null!;
    }

}
