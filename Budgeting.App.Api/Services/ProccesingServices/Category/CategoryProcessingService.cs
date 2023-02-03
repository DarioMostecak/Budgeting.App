using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.OperationResults;
using Budgeting.App.Api.Services.Foundations.Categories;

namespace Budgeting.App.Api.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService : ICategoryProcessingService
    {
        private readonly ICategoryService service;
        private readonly ILoggingBroker loggingBroker;

        public CategoryProcessingService(ICategoryService service,
            ILoggingBroker loggingBroker)
        {
            this.service = service;
            this.loggingBroker = loggingBroker;
        }

        public OperationResult<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var operationResult = new OperationResult<IEnumerable<CategoryDto>>();
            return TryCatch(operationResult, () =>
            {
                operationResult.Payload = service.RetrieveAllCategories();

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var operationResult = new OperationResult<CategoryDto>();
            return TryCatch(operationResult, async () =>
            {
                var newCategory = await service.CreateCategoryAsync(categoryDto);
                operationResult.Payload = newCategory;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var operationResult = new OperationResult<CategoryDto>();
            return TryCatch(operationResult, async () =>
            {
                var updateCategorViewModel = await service.ModifyCategoryAsync(categoryDto);
                operationResult.Payload = updateCategorViewModel;

                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryDto>> DeleteCategoryAsync(Guid id)
        {
            var operationResult = new OperationResult<CategoryDto>();
            return TryCatch(operationResult, async () =>
            {
                operationResult.Payload = await service.RemoveCategoryByIdAsync(id);
                return operationResult;
            });
        }

        public ValueTask<OperationResult<CategoryDto>> GetCategoryById(Guid categoryId)
        {
            var operationResult = new OperationResult<CategoryDto>();
            return TryCatch(operationResult, async () =>
            {
                operationResult.Payload = await service.RetriveCategoryByIdAsync(categoryId);
                return operationResult;
            });
        }
    }
}
