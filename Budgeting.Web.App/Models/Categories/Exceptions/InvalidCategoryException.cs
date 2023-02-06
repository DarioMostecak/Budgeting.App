namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class InvalidCategoryException : Exception
    {

        public InvalidCategoryException()
            : base(message: "Invalid category.") { }

        public InvalidCategoryException(Guid parameterId)
            : base(message: $"Invalid category input. Category id not same as {parameterId}") { }


        public InvalidCategoryException(string parameterName, object parameterValue)
            : base(message: $"Invalid category input. Parameter name: {parameterName}, Parameter value: {parameterValue}") { }


        public List<(string, string)> ValidationErrors = new List<(string, string)>();
    }
}
