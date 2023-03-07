using Budgeting.App.Api.Tests.Acceptance.Models.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budgeting.App.Api.Tests.Acceptance.Brokers
{
    public partial class BudgetingAppApiBroker
    {
        private const string CategoryiesRelativeUrl = "api/v1/Categories";

        public async ValueTask<List<Category>> GetAllCategoriesAsync() =>
            await this.GetContentAsync<List<Category>>(CategoryiesRelativeUrl);

        public async ValueTask<Category> PostCategoryAsync(Category category) =>
            await this.PostContentAsync<Category>(CategoryiesRelativeUrl, category);

        public async ValueTask<Category> GetCategoryByIdAsync(Guid categoryId) =>
            await this.GetContentAsync<Category>($"{CategoryiesRelativeUrl}/{categoryId}");

        public async ValueTask<Category> PutCategoryAsync(Category category) =>
            await this.PutContentAsync<Category>(CategoryiesRelativeUrl, category);

        public async ValueTask<Category> DeleteCategoryAsync(Guid categoryId) =>
            await this.DeleteContentAsync<Category>($"{CategoryiesRelativeUrl}/{categoryId}");
    }
}
