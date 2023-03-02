using Budgeting.App.Api.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class CategoryValidationException : ExceptionModel
    {
        public CategoryValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data)
        { }
    }
}
