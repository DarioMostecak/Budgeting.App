using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;

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

        public ValueTask<CategoryDto> AddCategoryAsync(CategoryDto categoryDto) =>
        TryCatch(async () =>
        {
            Category newCategory = categoryDto;
            ValidateCategoryOnCreate(newCategory);
            await this.storageBroker.InsertCategoryAsync(newCategory);

            return (CategoryDto)newCategory;

        });

        public IQueryable<CategoryDto> RetrieveAllCategories() =>
        TryCatch(() =>
        {
            var listCategory = this.storageBroker.SelectAllCategories().ToList();
            var listCategoryDto = listCategory.ConvertAll(category => (CategoryDto)category);

            return listCategoryDto.AsQueryable();
        });

        public ValueTask<CategoryDto> RetriveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            return (CategoryDto)maybeCategory;
        });

        public ValueTask<CategoryDto> ModifyCategoryAsync(CategoryDto categoryDto) =>
        TryCatch(async () =>
        {
            Category inputCategory = categoryDto;
            ValidateCategoryOnModify(inputCategory);

            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(inputCategory.CategoryId);
            ValidateStorageCategory(storageCategory: maybeCategory, inputCategory.CategoryId);
            ValidateAgainstStorageCategoryOnModify(inputCategory: inputCategory, storageCategory: maybeCategory);

            var updateCategory = await this.storageBroker.UpdateCategoryAsync(inputCategory);
            return (CategoryDto)updateCategory;
        });

        public ValueTask<CategoryDto> RemoveCategoryByIdAsync(Guid categoryId) =>
        TryCatch(async () =>
        {
            ValidateCategoryIdIsNull(categoryId);
            Category maybeCategory = await this.storageBroker.SelectCategoriesByIdAsync(categoryId);
            ValidateStorageCategory(maybeCategory, categoryId);

            var deletedCategory = await this.storageBroker.DeleteCategoryAsync(maybeCategory);
            return (CategoryDto)deletedCategory;
        });
    }
}
