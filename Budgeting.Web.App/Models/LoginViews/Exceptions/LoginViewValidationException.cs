using Budgeting.Web.App.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class LoginViewValidationException : ExceptionModel
    {
        public LoginViewValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException: innerException, data)
        { }
    }
}
