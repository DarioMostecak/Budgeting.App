namespace Budgeting.App.Api.Models.Exceptions
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException()
            : base(message: "Invalid category. Please fix the errors and try again.") { }

        public InvalidCategoryException(string parameterName, object parameterValue)
            : base(message: $"Invalid assignment, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }


        public List<(string, string)> ValidationErrors = new List<(string, string)>();
    }
}
