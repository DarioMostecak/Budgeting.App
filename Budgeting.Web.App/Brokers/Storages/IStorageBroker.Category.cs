using Budgeting.Web.App.Models;

namespace Budgeting.Web.App.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<List<Category>> SelectAllCategoriesAsync();
        ValueTask<Category> InsertCategoryAsync(Category category);
        ValueTask<Category> SelectCategoriesByIdAsync(Guid categoryId);
        ValueTask<Category> UpdateCategoryAsync(Category category);
        ValueTask<Category> DeleteCategoryAsync(Guid categoryId);
    }
}
