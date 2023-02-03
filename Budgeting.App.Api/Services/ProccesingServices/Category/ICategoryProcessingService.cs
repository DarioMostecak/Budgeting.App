using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.OperationResults;

namespace Budgeting.App.Api.Services.ProcessingServices.CategoryProcessingServices
{
    public interface ICategoryProcessingService
    {
        OperationResult<IEnumerable<CategoryDto>> GetAllCategories();
        ValueTask<OperationResult<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto);
        ValueTask<OperationResult<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto);
        ValueTask<OperationResult<CategoryDto>> DeleteCategoryAsync(Guid id);
        ValueTask<OperationResult<CategoryDto>> GetCategoryById(Guid id);
    }
}
