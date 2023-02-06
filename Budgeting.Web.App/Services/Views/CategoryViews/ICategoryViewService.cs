using Budgeting.Web.App.Models.CategoryViews;

namespace Budgeting.Web.App.Services.Views.CategoryViews
{
    public interface ICategoryViewService
    {
        ValueTask<List<CategoryView>> GetAllCategoriesAsync();
        ValueTask CreateCategoryAsync(CategoryView categoryViewModel);
        ValueTask UpdateCategoryAsync(CategoryView categoryViewModel);
        ValueTask DeleteCategoryAsync(Guid id);
        ValueTask<CategoryView> GetCategoryById(string id);
    }
}
