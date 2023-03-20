using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class InvalidCategoryException : ExceptionModel
    {
        public InvalidCategoryException()
            : base(message: "Invalid category. Please fix the errors and try again.") { }

        public InvalidCategoryException(string parameterName, object parameterValue)
          : base(message: $"Invalid category, " +
                $"parameter name: {parameterName}, " +
                $"parameter value: {parameterValue}.")
        { }

    }
}
