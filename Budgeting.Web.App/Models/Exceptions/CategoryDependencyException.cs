using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class CategoryDependencyException : Exception
    {
        public CategoryDependencyException(Exception innerException) :
            base(message: CategoryExceptionErrorMessages.CategoryDependencyExceptionErrorMessage, innerException)
        { }
    }
}
