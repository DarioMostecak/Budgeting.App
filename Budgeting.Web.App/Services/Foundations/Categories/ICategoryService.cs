using Budgeting.Web.App.Contracts;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<CategoryViewModel> CreateCategoryAsync(CategoryViewModel categoryViewModel);
        public IQueryable<CategoryViewModel> RetrieveAllCategories();
        ValueTask<CategoryViewModel> RetriveCategoryByIdAsync(Guid categoryId);
        ValueTask<CategoryViewModel> ModifyCategoryAsync(CategoryViewModel categoryViewModel);
        ValueTask<CategoryViewModel> RemoveCategoryByIdAsync(Guid categoryId);
    }
}
