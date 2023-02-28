using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class CategoryValidationException : ExceptionModel
    {
        public CategoryValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException)
        { }
    }
}
