using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class InvalidIdentityRequestException : ExceptionModel
    {
        public InvalidIdentityRequestException()
            : base(message: "Invalid identity request. Please fix errors and try again.") { }

        public InvalidIdentityRequestException(string parameterName, object parameterValue)
            : base(message: $"Invalid identity request " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
