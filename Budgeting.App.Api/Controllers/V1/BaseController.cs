using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.OperationResults;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleErrorResponse<T>(OperationResult<T> operationResult)
        {

            var apiError = new ErrorResponse();

            switch (operationResult.ErrorCode)
            {
                case ErrorCode.Validation:
                    apiError.StatusCode = (int)operationResult.ErrorCode;
                    apiError.Message = operationResult.Message;
                    foreach (var error in operationResult.Errors)
                    {
                        apiError.Errors.Add(error.Message);
                    }
                    apiError.Timestamp = DateTime.UtcNow;

                    return BadRequest(apiError);

                case ErrorCode.Dependency:
                    apiError.StatusCode = (int)operationResult.ErrorCode;
                    apiError.Message = operationResult.Message;
                    apiError.Timestamp = DateTime.UtcNow;

                    return StatusCode(503, apiError);

                default:
                    apiError.StatusCode = (int)operationResult.ErrorCode;
                    apiError.Message = operationResult.Message;
                    apiError.Timestamp = DateTime.UtcNow;

                    return StatusCode(500, apiError);
            }


        }
    }
}
