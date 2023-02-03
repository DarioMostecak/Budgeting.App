using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class CategoryDependencyException : Exception
    {
        public CategoryDependencyException(Exception innerException) :
            base(message: CategoryExceptionErrorMessages.CategoryDependencyExceptionErrorMessage, innerException)
        { }
    }
}
