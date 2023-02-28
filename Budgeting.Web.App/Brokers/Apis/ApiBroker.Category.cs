using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker
    {
        private const string relativeUrl = "Categories";

        public async ValueTask<List<Category>> SelectAllCategoriesAsync()
        => await this.GetAsync<List<Category>>(relativeUrl);

        public async ValueTask<Category> InsertCategoryAsync(Category category)
        => await this.PostAsync(relativeUrl, category);

        public async ValueTask<Category> SelectCategoriesByIdAsync(Guid categoryId)
        => await this.GetAsync<Category>(relativeUrl + $"/{categoryId}");

        public async ValueTask<Category> UpdateCategoryAsync(Category category)
        => await this.PutAsync(relativeUrl, category);

        public async ValueTask<Category> DeleteCategoryAsync(Guid categoryId)
        => await this.DeleteAsync<Category>(relativeUrl + $"/{categoryId}");


    }
}
