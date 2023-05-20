// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial interface IApiBroker
    {
        ValueTask<IEnumerable<Category>> GetCategoriesAsync();
        ValueTask<Category> GetCategoryAsync(string id);
        ValueTask<Category> PostCategoryAsync(Category category);
        ValueTask<Category> PutCategoryAsync(Category category);
        ValueTask<Category> DeleteCategoryAsync(string id);
    }
}
