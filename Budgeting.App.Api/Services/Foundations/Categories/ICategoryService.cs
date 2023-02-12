using Budgeting.App.Api.Contracts;

namespace Budgeting.App.Api.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<CategoryDto> AddCategoryAsync(CategoryDto categoryDto);
        IQueryable<CategoryDto> RetrieveAllCategories();
        ValueTask<CategoryDto> RetriveCategoryByIdAsync(Guid categoryId);
        ValueTask<CategoryDto> ModifyCategoryAsync(CategoryDto categoryDto);
        ValueTask<CategoryDto> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
