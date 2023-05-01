using Budgeting.Web.App.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class UserViewValidationException : ExceptionModel
    {
        public UserViewValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data: data) { }
    }
}
