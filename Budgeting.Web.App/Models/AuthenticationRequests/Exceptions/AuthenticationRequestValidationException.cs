using Budgeting.Web.App.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class AuthenticationRequestValidationException : ExceptionModel
    {
        public AuthenticationRequestValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException: innerException)
        { }
    }
}
