namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class FailedCategoryViewServiceException : Exception
    {
        public FailedCategoryViewServiceException(Exception innerException)
            : base(message: "Failed category view service error occurred, please contact support", innerException) { }

    }
}
