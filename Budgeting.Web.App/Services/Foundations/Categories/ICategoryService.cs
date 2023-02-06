using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<Category> CreateCategoryAsync(Category category);
        ValueTask<List<Category>> RetrieveAllCategoriesAsync();
        ValueTask<Category> RetriveCategoryByIdAsync(Guid categoryId);
        ValueTask<Category> ModifyCategoryAsync(Category category);
        ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
