using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Shared.ErrorResponseObjects
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object? value) : base(value) =>
            StatusCode = StatusCodes.Status500InternalServerError;
    }
}
