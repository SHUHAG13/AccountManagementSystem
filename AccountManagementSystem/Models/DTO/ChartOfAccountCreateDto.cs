using System.ComponentModel.DataAnnotations;

namespace AccountManagementSystem.Models.DTO
{
    public class ChartOfAccountCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string AccountName { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }
}
