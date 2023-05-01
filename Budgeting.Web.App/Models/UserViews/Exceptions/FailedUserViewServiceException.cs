namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class FailedUserViewServiceException : Exception
    {
        public FailedUserViewServiceException(Exception innerException)
            : base(message: "Failed service error occured. Contact support.", innerException)
        { }
    }
}
