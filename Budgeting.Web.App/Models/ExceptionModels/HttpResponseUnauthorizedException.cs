using Budgeting.Web.App.Models.ValidationProblemDetails;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseUnauthorizedException : HttpResponseException
    {
        public HttpResponseUnauthorizedException(HttpResponseMessage responseMessage, string message)
            : base(responseMessage, message) { }

        public HttpResponseUnauthorizedException(
            HttpResponseMessage responseMessage,
            ValidationProblemDetail problemDetails) : base(responseMessage, problemDetails.Title)
        {
            this.AddData((IDictionary)problemDetails.Errors);
        }
    }
}
