using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;
using System.Text.RegularExpressions;

namespace Budgeting.Web.App.Services.Views.UserViews
{
    public partial class UserViewService
    {
        private static void ValidateUserViewOnAdd(UserView userView)
        {
            ValidateUserViewIsNull(userView);
            Validate(
                (Rule: IsInvalidX(userView.FirstName), Parameter: nameof(UserView.FirstName)),
                (Rule: IsInvalidX(userView.LastName), Parameter: nameof(UserView.LastName)),
                (Rule: IsInvalidEmail(userView.Email), Parameter: nameof(UserView.Email)),
                (Rule: IsInvalidPassword(userView.Password), Parameter: nameof(UserView.Password)),

                (Rule: IsNotSamePassword(
                    userView.Password,
                    userView.ConfirmPassword),
                 Parameter: nameof(UserView.ConfirmPassword))
                );

        }

        private static dynamic IsInvalidPassword(string password) => new
        {
            Condition = string.IsNullOrWhiteSpace(password)
             || password.Count() < 8
               || password.Count() > 25
                 || Regex.IsMatch(password, @"(\s)"),

            Message = "Password must be between 8 and 25 charachters long, can't be white space or contain white space."
        };

        public static dynamic IsNotSamePassword(
            string confirmPassword,
            string password) => new
            {
                Condition = confirmPassword != password,
                Message = "Confirm password must have same value as password."
            };

        private static dynamic IsInvalidEmail(string email) => new
        {
            Condition = string.IsNullOrWhiteSpace(email)
                        || !Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"),

            Message = "Email can't be white space or null and must be type of email format."
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text)
                          || (text.Length < 3
                              || text.Length > 20),

            Message = "Must be between 3 and 20 charachters long and can't be null or white space."
        };

        private static void ValidateRoute(string route)
        {
            if (IsInvalid(route))
            {
                throw new InvalidUserViewException(
                    parameterName: "Route",
                    parameterValue: route);
            }
        }

        private static bool IsInvalid(string text) => String.IsNullOrWhiteSpace(text);

        private static void ValidateUserViewIsNull(UserView userView)
        {
            if (userView is null)
                throw new NullUserViewException();
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserViewException =
                new InvalidUserViewException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserViewException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }
            invalidUserViewException.ThrowIfContainsErrors();
        }
    }
}
