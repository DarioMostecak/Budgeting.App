using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Categories;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IApiBroker apiBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IApiBroker apiBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.apiBroker = apiBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Category>> RetrieveAllCategoriesAsync() =>
        TryCatch(async () =>
        {
            return await this.apiBroker.SelectAllCategoriesAsync();
        });

        public ValueTask<Category> CreateCategoryAsync(Category category) =>
        TryCatch(async () =>
        {
            ValidateCategoryOnCreate(category);
            await this.apiBroker.InsertCategoryAsync(category);

            return category;
        });

        public ValueTask<Category> RetriveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.apiBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            return maybeCategory;
        });

        public ValueTask<Category> ModifyCategoryAsync(Category category) =>
        TryCatch(async () =>
        {
            ValidateCategoryOnModify(category);
            Category maybeCategory = await this.apiBroker.SelectCategoriesByIdAsync(category.CategoryId);
            ValidateStorageCategory(storageCategory: maybeCategory, category.CategoryId);
            ValidateAgainstStorageCategoryOnModify(inputCategory: category, storageCategory: maybeCategory);

            var updateCategory = await this.apiBroker.UpdateCategoryAsync(category);
            return updateCategory;
        });

        public ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.apiBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);
            var deletedCategory = await this.apiBroker.DeleteCategoryAsync(categoryId);

            return deletedCategory;
        });
    }
}
