// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker
    {
        public const string categoryRelativUrl = "";

        public async ValueTask<IEnumerable<Category>> GetCategoriesAsync() =>
            await this.GetContentAsync<IEnumerable<Category>>(categoryRelativUrl);

        public async ValueTask<Category> GetCategoryAsync(string categoryId) =>
            await this.GetContentAsync<Category>($"{categoryRelativUrl}/{categoryId}");

        public async ValueTask<Category> PostCategoryAsync(Category category) =>
            await this.PostContentAsync(categoryRelativUrl, category);

        public async ValueTask<Category> PutCategoryAsync(Category category) =>
            await this.PutContentAsync(categoryRelativUrl, category);

        public async ValueTask<Category> DeleteCategoryAsync(string categoryId) =>
            await this.DeleteContentAsync<Category>($"{categoryRelativUrl}/{categoryId}");
    }
}
