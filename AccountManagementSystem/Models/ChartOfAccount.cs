namespace MiniAccountApi.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = null!;

        public int? ParentId { get; set; }
        public ChartOfAccount? Parent { get; set; }   // 👈 Navigation back to parent

        public ICollection<ChartOfAccount> Children { get; set; } = new List<ChartOfAccount>(); // 👈 Navigation to children
    }

}
