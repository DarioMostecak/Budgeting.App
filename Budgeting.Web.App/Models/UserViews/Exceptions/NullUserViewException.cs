using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class NullUserViewException : ExceptionModel
    {
        public NullUserViewException()
            : base("User view is null.")
        { }
    }
}
