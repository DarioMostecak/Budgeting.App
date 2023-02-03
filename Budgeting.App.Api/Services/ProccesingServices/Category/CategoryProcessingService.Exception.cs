using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models.Exceptions;
using Budgeting.App.Api.OperationResults;

namespace Budgeting.App.Api.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService
    {
        private delegate ValueTask<OperationResult<CategoryDto>> ReturnigCategoryProcessingFunction();
        private delegate OperationResult<IEnumerable<CategoryDto>> ReturnigListCategoryProcessingFunction();

        private async ValueTask<OperationResult<CategoryDto>> TryCatch(
            OperationResult<CategoryDto> operationResult,
            ReturnigCategoryProcessingFunction returnigCategoryProcessingFunction)
        {
            try
            {
                return await returnigCategoryProcessingFunction();
            }
            catch (CategoryValidationException categoryValidationException)
                   when (categoryValidationException.ValidationErrorMessages.Count > 0)
            {
                return HandleValidationError(operationResult, ErrorCode.Validation, categoryValidationException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Validation, categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Dependency, categoryDependencyException);
            }
            catch (Exception exception)
            {
                return HandleExceptionError(operationResult, ErrorCode.Service, exception);
            }
        }

        private OperationResult<IEnumerable<CategoryDto>> TryCatch(
            OperationResult<IEnumerable<CategoryDto>> operationResult,
            ReturnigListCategoryProcessingFunction returnigListCategoryProcessingFunction)
        {
            try
            {
                return returnigListCategoryProcessingFunction();
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return HandleExceptionError(operationResult, ErrorCode.Dependency, categoryDependencyException);
            }
            catch (Exception exception)
            {
                return HandleExceptionError(operationResult, ErrorCode.Service, exception);
            }
        }


        private OperationResult<CategoryDto> HandleValidationError(
            OperationResult<CategoryDto> operationResult,
            ErrorCode errorCode,
            CategoryValidationException categoryValidationException)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;
            operationResult.Message = categoryValidationException.Message;

            foreach (var message in categoryValidationException.ValidationErrorMessages)
            {
                operationResult.Errors.Add(new Error { Message = message });
            }

            return operationResult;
        }

        private OperationResult<CategoryDto> HandleExceptionError(
             OperationResult<CategoryDto> operationResult,
             ErrorCode errorCode,
             Exception exception)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;
            operationResult.Message = exception.Message;

            return operationResult;
        }

        private OperationResult<IEnumerable<CategoryDto>> HandleExceptionError(
            OperationResult<IEnumerable<CategoryDto>> operationResult,
            ErrorCode errorCode,
            Exception exception)
        {
            operationResult.IsError = true;
            operationResult.ErrorCode = errorCode;
            operationResult.Message = exception.Message;

            return operationResult;

        }


    }
}
