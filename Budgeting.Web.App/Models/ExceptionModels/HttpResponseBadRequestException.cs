using Budgeting.Web.App.Models.ValidationProblemDetails;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseBadRequestException : HttpResponseException
    {
        public HttpResponseBadRequestException()
            : base(httpResponseMessage: default, message: default) { }

        public HttpResponseBadRequestException(HttpResponseMessage responseMessage, string message)
            : base(responseMessage, message) { }

        public HttpResponseBadRequestException(
            HttpResponseMessage responseMessage,
            ValidationProblemDetail problemDetails) : base(responseMessage, problemDetails.Title)
        {
            this.AddData((IDictionary)problemDetails.Errors);
        }
    }
}
