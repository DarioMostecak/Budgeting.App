using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityRequests.Exceptions;
using Budgeting.App.Api.Models.Users;
using System.Text.RegularExpressions;

namespace Budgeting.App.Api.Services.Identity
{
    public partial class IdentityService
    {
        public static void ValidateIdentityRequest(IdentityRequest identityRequest)
        {
            ValidateIdentityRequestIsNull(identityRequest);

            Validate(
                (Rule: IsInvalidEmail(identityRequest.Email), Parameter: nameof(IdentityRequest.Email)),
                (Rule: IsInvalidPassword(identityRequest.Password), Parameter: nameof(IdentityRequest.Password))
                );
        }

        private static dynamic IsInvalidPassword(string password) => new
        {
            Condition = string.IsNullOrWhiteSpace(password) || password.Count() < 8,
            Message = "Password is required."
        };

        private static dynamic IsInvalidEmail(string email) => new
        {
            Condition = string.IsNullOrWhiteSpace(email) ||
               !Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"),

            Message = "Email can't be white space or null and must be type of email format."
        };

        private static void ValidateIdentityRequestIsNull(IdentityRequest identityRequest)
        {
            if (identityRequest is null) throw new NullIdentityRequestException();
        }

        private static void ValidateIdentityIsNull(User user, string identityRequestEmail)
        {
            if (user is null)
                throw new FailAuthenticationIdentityRequestException(identityRequestEmail);
        }

        private static void ValidateIdentityPasswordConfirmationIsFalse(bool confirmPassword, string identityPassword)
        {
            if (confirmPassword == false) throw new FailAuthenticationIdentityRequestException(identityPassword);
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidIdentityRequestException = new InvalidIdentityRequestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIdentityRequestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }

            invalidIdentityRequestException.ThrowIfContainsErrors();
        }
    }
}
