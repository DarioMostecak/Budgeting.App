using Budgeting.App.Api.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class IdentityRequestValidationException : ExceptionModel
    {
        public IdentityRequestValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data)
        { }
    }
}
