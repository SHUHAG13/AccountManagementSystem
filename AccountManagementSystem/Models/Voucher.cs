namespace MiniAccountApi.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string VoucherType { get; set; } = null!;
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; } = null!;

        public ICollection<VoucherEntry> Entries { get; set; } = new List<VoucherEntry>();
    }
}
