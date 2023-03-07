using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Models.Categories;

namespace Budgeting.App.Api.Services.Foundations.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CategoryService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Category> AddCategoryAsync(Category category) =>
        TryCatch(async () =>
        {
            ValidateCategoryOnCreate(category);

            return await this.storageBroker.InsertCategoryAsync(category);
        });

        public IQueryable<Category> RetrieveAllCategories() =>
        TryCatch(() => this.storageBroker.SelectAllCategories());

        public ValueTask<Category> RetriveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(storageCategory: maybeCategory, categoryId);

            return maybeCategory;
        });

        public ValueTask<Category> ModifyCategoryAsync(Category category) =>
        TryCatch(async () =>
        {

            ValidateCategoryOnModify(category);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(category.CategoryId);

            ValidateStorageCategory(storageCategory: maybeCategory, category.CategoryId);
            ValidateAgainstStorageCategoryOnModify(inputCategory: category, storageCategory: maybeCategory);

            var updateCategory = await this.storageBroker.UpdateCategoryAsync(category);

            return updateCategory;
        });

        public ValueTask<Category> RemoveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            var deletedCategory = await this.storageBroker.DeleteCategoryAsync(maybeCategory);

            return deletedCategory;
        });
    }
}
