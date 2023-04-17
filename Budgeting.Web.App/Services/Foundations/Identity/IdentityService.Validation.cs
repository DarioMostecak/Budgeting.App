using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;

namespace Budgeting.Web.App.Services.Foundations
{
    public partial class IdentityService
    {
        private static void ValidateAuthenticationRequest(AuthenticationRequest authenticationRequest)
        {
            ValidateAuthenticationRequestIsNull(authenticationRequest);

            Validate(
                (Rule: IsInvalidEmail(authenticationRequest.Email), Parameter: nameof(AuthenticationRequest.Email)),
                (Rule: IsInvalidPassword(authenticationRequest.Password), Parameter: nameof(AuthenticationRequest.Password))
                );
        }

        private static dynamic IsInvalidEmail(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Email is required.",
        };

        private static dynamic IsInvalidPassword(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Password is required.",
        };

        private static void ValidateAuthenticationRequestIsNull(AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest is null)
                throw new NullAuthenticationRequestException();
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAuthenticationRequestException =
                new InvalidAuthenticationRequestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAuthenticationRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }
            invalidAuthenticationRequestException.ThrowIfContainsErrors();
        }
    }
}
