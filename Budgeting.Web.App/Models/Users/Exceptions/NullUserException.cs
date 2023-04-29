using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class NullUserException : ExceptionModel
    {
        public NullUserException()
            : base(message: "User is null!")
        { }

    }
}
