using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private static void ValidateUserOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
            ValidatePasswordIsNull(password);

            Validate(
                (Rule: IsInvalidX(user.Id), Parameter: nameof(User.Id)),
                (Rule: IsInvalidX(user.FirstName), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalidX(user.LastName), Parameter: nameof(User.LastName)),
                (Rule: IsInvalidEmail(user.Email), Parameter: nameof(User.Email)),
                (Rule: IsInvalidX(user.CreatedDate), Parameter: nameof(User.CreatedDate)),
                (Rule: IsInvalidX(user.UpdatedDate), Parameter: nameof(User.UpdatedDate)),
                (Rule: IsInvalidPassword(password), Parameter: nameof(password))
                );
        }

        private static void ValidateUserOnModify(User user)
        {
            ValidateUserIsNull(user);
        }

        private static dynamic IsInvalidX(Guid applicationUserId) => new
        {
            Condition = applicationUserId == Guid.Empty,
            Message = "Id isn't valid."
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text) || (text.Length < 3 || text.Length > 20),
            Message = "Must be between 3 and 20 charachters long and can't be null or white space."
        };

        private static dynamic IsInvalidX(DateTime date) => new
        {
            Condition = date == default,
            Message = "Date is required."
        };

        private static dynamic IsInvalidPassword(string password) => new
        {
            Condition = string.IsNullOrWhiteSpace(password)
             || password.Count() < 8
               || password.Count() > 25
                 || Regex.IsMatch(password, @"(\s)"),

            Message = "Password must be between 8 and 25 charachters long, can't be white space or contain white space."
        };

        private static dynamic IsInvalidEmail(string email) => new
        {
            Condition = string.IsNullOrWhiteSpace(email) ||
               !Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"),

            Message = "Email can't be white space or null and must be type of email format."
        };

        private static void ValidateUserIsNull(User user)
        {
            if (user is null) throw new NullUserException();
        }

        private static void ValidatePasswordIsNull(string password)
        {
            if (password is null) throw new NullUserPasswordException();
        }

        private static void ValidateUserIsNotNull(User user)
        {
            if (user != null) throw new AlreadyExistsUserException();
        }

        private static void ValidateStorageUser(Guid userId, User user)
        {
            if (user is null)
                throw new NotFoundUserException(userId);
        }

        private static bool IsValid(Guid id) => id == Guid.Empty;

        private static void ValidateUserIdIsNull(Guid Id)
        {
            if (IsValid(Id))
            {
                var invalidUserException =
                    new InvalidUserException(
                       parameterName: nameof(Id),
                       parameterValue: Id);

                throw invalidUserException;
            }
        }

        private static void ValidateIdentityResultIsFalse(IdentityResult identityResult)
        {
            if (identityResult.Succeeded == false)
            {
                var invalidUserException = new InvalidUserException();

                foreach (var error in identityResult.Errors)
                {
                    invalidUserException.UpsertDataList(
                        key: error.Code,
                        value: error.Description);
                }

                throw invalidUserException;
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserException = new InvalidUserException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }

            invalidUserException.ThrowIfContainsErrors();
        }
    }
}
