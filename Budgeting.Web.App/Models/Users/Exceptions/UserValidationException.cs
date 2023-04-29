using Budgeting.Web.App.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class UserValidationException : ExceptionModel
    {
        public UserValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data: data) { }
    }
}
