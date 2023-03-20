namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class FailedUserServiceException : Exception
    {
        public FailedUserServiceException(Exception innerException)
            : base(message: "Failed user service exception, contact support.", innerException)
        { }
    }
}
