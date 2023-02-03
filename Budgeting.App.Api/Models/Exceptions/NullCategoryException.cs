using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException()
            : base(message: string.Format(CategoryExceptionErrorMessages.NullCategoryExceptionErrorMessage)) { }
    }
}
