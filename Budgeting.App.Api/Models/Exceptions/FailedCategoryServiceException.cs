namespace Budgeting.App.Api.Models.Exceptions
{
    public class FailedCategoryServiceException : Exception
    {
        public FailedCategoryServiceException(Exception innerException) :
           base(message: "Failed category service exception, contact support.", innerException)
        { }
    }
}
