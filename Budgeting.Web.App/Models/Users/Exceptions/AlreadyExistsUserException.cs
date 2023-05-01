using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class AlreadyExistsUserException : ExceptionModel
    {
        public AlreadyExistsUserException(Exception innerException)
           : base(message: "User with same id already exists.", innerException)
        { }
    }
}
