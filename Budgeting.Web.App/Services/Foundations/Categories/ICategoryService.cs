using Budgeting.Web.App.Contracts;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<CategoryViewModel> CreateCategoryAsync(CategoryViewModel categoryViewModel);
        ValueTask<List<CategoryViewModel>> RetrieveAllCategoriesAsync();
        ValueTask<CategoryViewModel> RetriveCategoryByIdAsync(Guid categoryId);
        ValueTask<CategoryViewModel> ModifyCategoryAsync(CategoryViewModel categoryViewModel);
        ValueTask<CategoryViewModel> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
