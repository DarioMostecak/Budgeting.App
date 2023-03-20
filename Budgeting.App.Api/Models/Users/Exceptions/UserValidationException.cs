using Budgeting.App.Api.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class UserValidationException : ExceptionModel
    {
        public UserValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data)
        { }
    }
}
