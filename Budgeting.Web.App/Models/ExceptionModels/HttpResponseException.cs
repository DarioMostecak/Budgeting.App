namespace Budgeting.Web.App.Models.ExceptionModels
{
    public class HttpResponseException : ExceptionModel
    {
        public HttpResponseException(HttpResponseMessage httpResponseMessage, string message)
            : base(message) => this.HttpResponseMessage = httpResponseMessage;

        public HttpResponseMessage HttpResponseMessage { get; private set; }
    }
}
