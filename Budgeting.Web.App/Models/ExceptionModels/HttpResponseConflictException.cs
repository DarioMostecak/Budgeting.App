using Budgeting.Web.App.Models.ValidationProblemDetails;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseConflictException : HttpResponseException
    {
        public HttpResponseConflictException()
            : base(httpResponseMessage: default, message: default) { }

        public HttpResponseConflictException(HttpResponseMessage responseMessage, string message)
            : base(responseMessage, message) { }

        public HttpResponseConflictException(
            HttpResponseMessage responseMessage,
            ValidationProblemDetail problemDetail) : base(responseMessage, problemDetail.Title)
        {
            this.AddData((IDictionary)problemDetail.Errors);
        }
    }
}
