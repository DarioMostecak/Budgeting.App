using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.OperationResults;
using Budgeting.Web.App.Services.Foundations.Categories;

namespace Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService : ICategoryProcessingService
    {
        private readonly ICategoryService service;

        public CategoryProcessingService(ICategoryService service)
        {
            this.service = service;
        }

        public OperationResult<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            var operationResult = new OperationResult<IEnumerable<CategoryViewModel>>();
            return TryCatch(operationResult, () =>
            {
                var categoryViewModelList = service.RetrieveAllCategories();
                operationResult.Payload = categoryViewModelList.ToList();

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> CreateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var newCategoryViewModel = await service.CreateCategoryAsync(categoryViewModel);
                operationResult.Payload = newCategoryViewModel;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> UpdateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var updateCategorViewModel = await service.ModifyCategoryAsync(categoryViewModel);
                operationResult.Payload = updateCategorViewModel;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryViewModel>> DeleteCategoryAsync(Guid id)
        {
            var operationResult = new OperationResult<CategoryViewModel>();
            return TryCatch(operationResult, async () =>
            {
                var deletedCategoryViewModel = await service.RemoveCategoryByIdAsync(id);
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
                var categoryViewModel = await service.RetriveCategoryByIdAsync(new Guid(categoryId));
                operationResult.Payload = categoryViewModel;
                return operationResult;
            });
        }
    }
}
