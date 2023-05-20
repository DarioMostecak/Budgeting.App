// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        ValueTask<IEnumerable<Category>> RetrieveAllCategoriesAsync();
        ValueTask<Category> GetCategoryById(Category category);
        ValueTask<Category> AddCategoryAsync(Category category);
        ValueTask<Category> ModifyCategoryAsync(Category category);
        ValueTask<Category> DeleteCategoryAsync(Category category);
    }
}
