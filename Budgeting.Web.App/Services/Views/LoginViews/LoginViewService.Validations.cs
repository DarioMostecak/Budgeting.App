using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using System.Text.RegularExpressions;

namespace Budgeting.Web.App.Services.Views.LoginViews
{
    public partial class LoginViewService
    {
        private static void ValidateLoginView(LoginView loginView)
        {
            ValidateLoginViewIsNull(loginView);

            Validate(
                (Rule: IsInvalidEmail(loginView.Email), Parameter: nameof(LoginView.Email)),
                (Rule: IsInvalidPassword(loginView.Password), Parameter: nameof(LoginView.Password))
                );
        }

        private static dynamic IsInvalidPassword(string password) => new
        {
            Condition = string.IsNullOrWhiteSpace(password) || password.Count() < 8,
            Message = "Password is required and must be at least 8 character long."
        };

        private static dynamic IsInvalidEmail(string email) => new
        {
            Condition = string.IsNullOrWhiteSpace(email) ||
               !Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"),

            Message = "Email can't be white space or null and must be type of email format."
        };

        private static void ValidateRoute(string route)
        {
            if (IsInvalid(route))
                throw new InvalidLoginViewException(
                    parameterName: "route",
                    parameterValue: route);
        }

        private static bool IsInvalid(string text) => string.IsNullOrWhiteSpace(text);

        private static void ValidateLoginViewIsNull(LoginView loginView)
        {
            if (loginView is null)
                throw new NullLoginViewException();
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLoginViewException =
                new InvalidLoginViewException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLoginViewException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }
            invalidLoginViewException.ThrowIfContainsErrors();
        }
    }
}
