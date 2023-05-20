using Budgeting.App.Api.Models.Categories;

namespace Budgeting.App.Api.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        IQueryable<Category> RetrieveAllCategories();
        ValueTask<Category> AddCategoryAsync(Category category);
        ValueTask<Category> RetriveCategoryByIdAsync(Guid categoryId);
        ValueTask<Category> ModifyCategoryAsync(Category category);
        ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
