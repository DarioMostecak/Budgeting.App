using Budgeting.Web.App.Models.ValidationProblemDetails;
using System.Collections;

namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseInternalServerErrorException : HttpResponseException
    {
        public HttpResponseInternalServerErrorException(HttpResponseMessage responseMessage, string message)
           : base(responseMessage, message) { }

        public HttpResponseInternalServerErrorException(
            HttpResponseMessage responseMessage,
            ValidationProblemDetail problemDetail) : base(responseMessage, problemDetail.Title)
        {
            this.AddData((IDictionary)problemDetail.Errors);
        }
    }
}
