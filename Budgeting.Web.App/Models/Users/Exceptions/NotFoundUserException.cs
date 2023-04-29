using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class NotFoundUserException : ExceptionModel
    {
        public NotFoundUserException(string userId)
            : base(message: $"Can't find user with id {userId}.")
        { }

    }
}
