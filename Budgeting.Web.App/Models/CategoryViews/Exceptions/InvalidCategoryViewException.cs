namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class InvalidCategoryViewException : Exception
    {
        public InvalidCategoryViewException(string parameterName, object parameterValue)
            : base(message: $"Invalid category input. Parameter name: {parameterName}, Parameter value: {parameterValue}.") { }
    }
}
