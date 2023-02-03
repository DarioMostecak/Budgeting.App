namespace Budgeting.App.Api.Models.Exceptions.ErrorMessages
{
    public static class CategoryExceptionErrorMessages
    {
        public const string CategoryNotFoundExceptionErrorMessage = "Category with {0} not found.";
        public const string NullCategoryExceptionErrorMessage = "Category is null";
        public const string InvalidCategoryExceptionErrorMessage = "Invalid category.";
        public const string InvalidCategoryExceptionErrorOneParametersMessage = "Invalid category input. Category id not same as {0}.";
        public const string InvalidCategoryExceptionErrorTwoParametersMessage = "Invalid category input. Parameter name: {0}, Parameter value: {1}";
        public const string FailedCategoryServiceExceptionErrorMessage = "Failed category service exception, contact support.";
        public const string CategoryDependencyExceptionErrorMessage = "Service dependency error occurred, contact support.";
        public const string CategoryServiceExceptionErrorMessage = "Service error occurred, contact support.";
        public const string AlredyExistsCategoryExceptionErrorMessage = "Category with same id already exists.";
    }
}
