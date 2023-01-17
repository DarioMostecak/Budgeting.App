using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Storages;
using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CategoryViewModel> CreateCategoryAsync(CategoryViewModel categoryViewModel) =>
        TryCatch(async () =>
        {
            Category newCategory = categoryViewModel;
            newCategory.SetTimeCreatedAndTimeModify();

            ValidateCategoryOnCreate(newCategory);
            await this.storageBroker.InsertCategoryAsync(newCategory);

            return (CategoryViewModel)newCategory;

        });

        public IQueryable<CategoryViewModel> RetrieveAllCategories() =>
        TryCatch(() =>
        {
            var listCategory = this.storageBroker.SelectAllCategories().ToList();
            var listCategoryViewModel = listCategory.ConvertAll(category => (CategoryViewModel)category);

            return listCategoryViewModel.AsQueryable();
        });

        public ValueTask<CategoryViewModel> RetriveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            return (CategoryViewModel)maybeCategory;
        });

        public ValueTask<CategoryViewModel> ModifyCategoryAsync(CategoryViewModel categoryViewModel) =>
        TryCatch(async () =>
        {
            Category inputCategory = categoryViewModel;
            inputCategory.UpdateTimeModify();
            ValidateCategoryOnModify(inputCategory);

            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(inputCategory.CategoryId);
            ValidateStorageCategory(storageCategory: maybeCategory, inputCategory.CategoryId);
            ValidateAgainstStorageCategoryOnModify(inputCategory: inputCategory, storageCategory: maybeCategory);

            var updateCategory = await this.storageBroker.UpdateCategoryAsync(inputCategory);
            return (CategoryViewModel)updateCategory;
        });

        public ValueTask<CategoryViewModel> RemoveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            var deletedCategory = await this.storageBroker.DeleteCategoryAsync(maybeCategory);
            return (CategoryViewModel)deletedCategory;
        });
    }
}
