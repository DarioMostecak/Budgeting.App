using Budgeting.Web.App.Models.ValidationProblemDetails;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseNotFoundException : HttpResponseException
    {
        public HttpResponseNotFoundException()
            : base(httpResponseMessage: default, message: default) { }

        public HttpResponseNotFoundException(HttpResponseMessage responseMessage, string message)
            : base(responseMessage, message) { }

        public HttpResponseNotFoundException(
            HttpResponseMessage responseMessage,
            ValidationProblemDetail problemDetail) : base(responseMessage, problemDetail.Title)
        {
            this.AddData((IDictionary)problemDetail.Errors);
        }
    }
}
