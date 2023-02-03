using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models.Exceptions;
using Budgeting.Web.App.OperationResults;

namespace Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService
    {
        private delegate ValueTask<OperationResult<CategoryViewModel>> ReturnigCategoryViewModelFunction();
        private delegate ValueTask<OperationResult<List<CategoryViewModel>>> ReturnigListCategoryViewModelsFunction();

        private async ValueTask<OperationResult<CategoryViewModel>> TryCatch(
            OperationResult<CategoryViewModel> operationResult,
            ReturnigCategoryViewModelFunction returnigCategoryViewModelFunction)
        {
            try
            {
                return await returnigCategoryViewModelFunction();
            }
            catch (CategoryValidationException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Validation);
            }
            catch (CategoryDependencyException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Dependency);
            }
            catch (Exception)
            {
                return HandleExceptionError(operationResult, ErrorCode.Service);
            }
        }

        private async ValueTask<OperationResult<List<CategoryViewModel>>> TryCatch(
            OperationResult<List<CategoryViewModel>> operationResult,
            ReturnigListCategoryViewModelsFunction returnigListCategoryViewModelsFunction)
        {
            try
            {
                return await returnigListCategoryViewModelsFunction();
            }
            catch (CategoryDependencyException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Dependency);
            }
            catch (Exception)
            {
                return HandleExceptionError(operationResult, ErrorCode.Service);
            }
        }



        private OperationResult<CategoryViewModel> HandleExceptionError(
             OperationResult<CategoryViewModel> operationResult, ErrorCode errorCode)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;

            return operationResult;
        }

        private OperationResult<List<CategoryViewModel>> HandleExceptionError(
            OperationResult<List<CategoryViewModel>> operationResult, ErrorCode errorCode)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;

            return operationResult;
        }
    }
}
