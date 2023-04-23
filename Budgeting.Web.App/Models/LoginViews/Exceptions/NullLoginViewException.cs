using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class NullLoginViewException : ExceptionModel
    {
        public NullLoginViewException()
            : base(message: "Login view is null.")
        { }
    }
}
