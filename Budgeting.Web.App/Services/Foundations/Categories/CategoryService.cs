// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IApiBroker apiBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IApiBroker apiBroker,
            ILoggingBroker loggingBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
        }


        public ValueTask<Category> AddCategoryAsync(Category category) =>
        TryCatch(async () =>
        {
            ValidateCategoryOnCreate(category);

            return await this.apiBroker.PostCategoryAsync(category);
        });

        public ValueTask<Category> DeleteCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<Category>> RetrieveAllCategoriesAsync() =>
        TryCatch(async () => await this.apiBroker.GetCategoriesAsync());


        public ValueTask<Category> RetrieveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);

            return await this.apiBroker.GetCategoryAsync(categoryId.ToString());
        });

        public ValueTask<Category> ModifyCategoryAsync(Category category) =>
        TryCatch(async () =>
        {
            ValidateCategoryOnModify(category);

            return await this.apiBroker.PutCategoryAsync(category);
        });

    }
}
