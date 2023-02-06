using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial interface IApiBroker
    {
        ValueTask<List<Category>> SelectAllCategoriesAsync();
        ValueTask<Category> InsertCategoryAsync(Category category);
        ValueTask<Category> SelectCategoriesByIdAsync(Guid categoryId);
        ValueTask<Category> UpdateCategoryAsync(Category category);
        ValueTask<Category> DeleteCategoryAsync(Guid categoryId);
    }
}
