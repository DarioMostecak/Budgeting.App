using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.OperationResults;
using Budgeting.Web.App.Services.Foundations.Categories;

namespace Budgeting.Web.App.Services.Views.CategoryViews
{
    public partial class CategoryViewService : ICategoryViewService
    {
        private readonly ICategoryService service;
        private readonly ILoggingBroker loggingBroker;

        public CategoryViewService(ICategoryService service,
            ILoggingBroker loggingBroker)
        {
            this.service = service;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OperationResult<List<CategoryViewModel>>> GetAllCategoriesAsync()
        {
            var operationResult = new OperationResult<List<CategoryViewModel>>();
            return TryCatch(operationResult, async () =>
            {
                var categoryViewModelList = await this.service.RetrieveAllCategoriesAsync();
                operationResult.Payload = categoryViewModelList;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> CreateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var newCategoryViewModel = await this.service.CreateCategoryAsync(categoryViewModel);
                operationResult.Payload = newCategoryViewModel;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> UpdateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var updateCategorViewModel = await this.service.ModifyCategoryAsync(categoryViewModel);
                operationResult.Payload = updateCategorViewModel;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> DeleteCategoryAsync(Guid id)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var deletedCategoryViewModel = await this.service.RemoveCategoryByIdAsync(id);
                operationResult.Payload = deletedCategoryViewModel;
                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> GetCategoryById(string categoryId)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                if (!IsGuidId(categoryId))
                {
                    operationResult.Payload = new CategoryViewModel();
                    return operationResult;
                }
                var categoryViewModel = await this.service.RetriveCategoryByIdAsync(new Guid(categoryId));
                operationResult.Payload = categoryViewModel;
                return operationResult;
            });
        }
    }
}
