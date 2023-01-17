using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException()
            : base(message: string.Format(CategoryExceptionErrorMessages.NullCategoryExceptionErrorMessage)) { }
    }
}
