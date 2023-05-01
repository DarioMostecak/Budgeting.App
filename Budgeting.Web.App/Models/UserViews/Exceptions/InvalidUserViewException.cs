using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class InvalidUserViewException : ExceptionModel
    {
        public InvalidUserViewException() :
            base(message: "Invalid user input, please fix errors and try again.")
        { }
    }
}
