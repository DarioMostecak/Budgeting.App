using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Storages;
using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CategoryService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<CategoryViewModel>> RetrieveAllCategoriesAsync() =>
        TryCatch(async () =>
        {
            var listCategories = await this.storageBroker.SelectAllCategoriesAsync();
            var listCategoryViewModels = listCategories.Select(category => (CategoryViewModel)category);

            return listCategoryViewModels.ToList();
        });

        public ValueTask<CategoryViewModel> CreateCategoryAsync(CategoryViewModel categoryViewModel) =>
        TryCatch(async () =>
        {
            var newCategory = MapToCategoryInsert(categoryViewModel);
            ValidateCategoryOnCreate(newCategory);
            await this.storageBroker.InsertCategoryAsync(newCategory);

            return (CategoryViewModel)newCategory;
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
            var inputCategory = MapToCategoryUpdate(categoryViewModel);
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
            var deletedCategory = await this.storageBroker.DeleteCategoryAsync(categoryId);

            return (CategoryViewModel)deletedCategory;
        });

        #region Private methods
        private Category MapToCategoryInsert(CategoryViewModel categoryViewModel)
        {
            DateTime currentDateTime = this.dateTimeBroker.GetCurrentDateTime();

            return new Category
            {
                CategoryId = Guid.NewGuid(),
                Title = categoryViewModel.Title,
                Icon = categoryViewModel.Icon,
                Type = categoryViewModel.Type,
                TimeCreated = currentDateTime,
                TimeModify = currentDateTime
            };
        }

        private Category MapToCategoryUpdate(CategoryViewModel categoryViewModel)
        {
            DateTime currentDateTime = this.dateTimeBroker.GetCurrentDateTime();

            return new Category
            {
                CategoryId = categoryViewModel.CategoryId,
                Title = categoryViewModel.Title,
                Icon = categoryViewModel.Icon,
                Type = categoryViewModel.Type,
                TimeCreated = categoryViewModel.TimeCreated,
                TimeModify = currentDateTime
            };
        }
        #endregion
    }
}
