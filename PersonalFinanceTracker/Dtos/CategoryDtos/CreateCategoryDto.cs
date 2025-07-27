using PersonalFinanceTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Dtos.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
    }
}
