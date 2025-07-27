using PersonalFinanceTracker.Dtos.CategoryDtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Mappers
{
    public static class CategoryMapper
    {
        public static Category FromCreateDtoToCategory(this CreateCategoryDto createDto) {
            return new Category { 
                Name = createDto.Name,
                Description = createDto.Description,
                PaymentType = createDto.PaymentType,
            };
        }
    }
}
