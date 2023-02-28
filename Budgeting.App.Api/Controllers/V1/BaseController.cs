using Budgeting.App.Api.Shared.ErrorResponseObjects;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Budgeting.App.Api.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public BadRequestObjectResult BadRequest(Exception exception)
        {
            var problemDetail = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = exception.Message
            };

            MapExceptionDataToProblemDetail(exception, problemDetail);

            return new BadRequestObjectResult(problemDetail);
        }

        [NonAction]
        public ConflictObjectResult Conflict(Exception exception)
        {
            var problemDetail = new ValidationProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                Title = exception.Message
            };

            return new ConflictObjectResult(problemDetail);
        }

        [NonAction]
        public NotFoundObjectResult NotFound(Exception exception)
        {
            var problemDetail = new ValidationProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = exception.Message
            };

            return new NotFoundObjectResult(problemDetail);
        }

        [NonAction]
        public InternalServerErrorObjectResult InternalServerError(Exception exception)
        {
            var problemDetail = new ValidationProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = exception.Message
            };

            return new InternalServerErrorObjectResult(problemDetail);
        }

        private void MapExceptionDataToProblemDetail(
            Exception exception,
            ValidationProblemDetails problemDetail)
        {
            foreach (DictionaryEntry error in exception.Data)
            {

                problemDetail.Errors.Add(
                    key: error.Key.ToString(),
                    value: ((List<string>)error.Value)?.ToArray());
            }
        }
    }
}
