using Budgeting.App.Api.Models;

namespace Budgeting.App.Api.Contracts
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }

        public static explicit operator CategoryDto(Category category)
        {
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Title = category.Title,
                Icon = category.Icon,
                Type = category.Type,
                TimeCreated = category.TimeCreated,
                TimeModify = category.TimeModify
            };
        }

        public static implicit operator Category(CategoryDto categoryDto)
        {
            return Category.CreateNewCategory(categoryDto.CategoryId, categoryDto.Title
                , categoryDto.Icon, categoryDto.Type
                , categoryDto.TimeCreated, categoryDto.TimeModify);
        }

    }
}
