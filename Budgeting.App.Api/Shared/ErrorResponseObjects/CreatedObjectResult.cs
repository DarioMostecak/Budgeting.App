using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Shared.ErrorResponseObjects
{
    public class CreatedObjectResult : ObjectResult
    {
        public CreatedObjectResult(object value) : base(value) =>
            StatusCode = StatusCodes.Status201Created;
    }
}
