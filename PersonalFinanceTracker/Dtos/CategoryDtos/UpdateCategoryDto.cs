using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Dtos.CategoryDtos
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
