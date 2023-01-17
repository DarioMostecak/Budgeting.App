using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models.Exceptions;
using Budgeting.Web.App.OperationResults;

namespace Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService
    {
        private delegate ValueTask<OperationResult<CategoryViewModel>> ReturnigCategoryProcessingFunction();
        private delegate OperationResult<IEnumerable<CategoryViewModel>> ReturnigListCategoryProcessingFunction();

        private async ValueTask<OperationResult<CategoryViewModel>> TryCatch(
            OperationResult<CategoryViewModel> operationResult,
            ReturnigCategoryProcessingFunction returnigCategoryProcessingFunction)
        {
            try
            {
                return await returnigCategoryProcessingFunction();
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

        private OperationResult<IEnumerable<CategoryViewModel>> TryCatch(
            OperationResult<IEnumerable<CategoryViewModel>> operationResult,
            ReturnigListCategoryProcessingFunction returnigListCategoryProcessingFunction)
        {
            try
            {
                return returnigListCategoryProcessingFunction();
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

        private OperationResult<IEnumerable<CategoryViewModel>> HandleExceptionError(
            OperationResult<IEnumerable<CategoryViewModel>> operationResult, ErrorCode errorCode)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;

            return operationResult;
        }
    }
}
