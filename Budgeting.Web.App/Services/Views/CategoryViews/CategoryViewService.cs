using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.CategoryViews;
using Budgeting.Web.App.Services.Foundations.Categories;

namespace Budgeting.Web.App.Services.Views.CategoryViews
{
    public partial class CategoryViewService : ICategoryViewService
    {
        private readonly ICategoryService service;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CategoryViewService(
            ICategoryService service,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.service = service;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<List<CategoryView>> GetAllCategoriesAsync() =>
        TryCatch(async () =>
        {
            var categories = await this.service.RetrieveAllCategoriesAsync();

            return categories.Select(AsCategoryViewModel).ToList();
        });


        public ValueTask CreateCategoryAsync(CategoryView categoryView) =>
        TryCatch(async () =>
        {
            ValidateCategoryViewIsNull(categoryView);
            var categoryToCreate = MapToCategoryInsert(categoryView);

            await this.service.CreateCategoryAsync(categoryToCreate);
        });


        public ValueTask UpdateCategoryAsync(CategoryView categoryView) =>
        TryCatch(async () =>
        {
            ValidateCategoryViewIsNull(categoryView);
            var categoryToUpdate = MapToCategoryUpdate(categoryView);

            await this.service.ModifyCategoryAsync(categoryToUpdate);
        });


        public ValueTask DeleteCategoryAsync(Guid id) =>
        TryCatch(async () =>
        {
            IsGuidDefault(id);

            await this.service.RemoveCategoryByIdAsync(id);
        });


        public ValueTask<CategoryView> GetCategoryById(string categoryId) =>
        TryCatch(async () =>
        {
            IsGuidValid(categoryId);

            if (IsGuidDefault(categoryId))
                return new CategoryView();

            var category = await this.service.RetriveCategoryByIdAsync(Guid.Parse(categoryId));
            var categoryView = MapToCategoryView(category);

            return categoryView;
        });



        #region Private methods

        private static Func<Category, CategoryView> AsCategoryViewModel =>
            category => new CategoryView
            {
                CategoryId = category.CategoryId,
                Title = category.Title,
                Icon = category.Icon,
                Type = category.Type,
                TimeCreated = category.TimeCreated,
                TimeModify = category.TimeModify
            };

        private Category MapToCategoryInsert(CategoryView categoryViewModel)
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

        private Category MapToCategoryUpdate(CategoryView categoryViewModel)
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

        private CategoryView MapToCategoryView(Category category)
        {
            return new CategoryView
            {
                CategoryId = category.CategoryId,
                Title = category.Title,
                Icon = category.Icon,
                Type = category.Type,
                TimeCreated = category.TimeCreated,
                TimeModify = category.TimeModify
            };
        }

        #endregion
    }
}
