namespace AccountManagementSystem.Models.DTO
{
    public class ChartOfAccountUpdateDto
    {
        public string AccountName { get; set; } = string.Empty;
        public int? ParentId { get; set; }
    }
}
