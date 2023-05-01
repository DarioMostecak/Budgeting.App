using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;

namespace Budgeting.Web.App.Services.Foundations.Users
{
    public partial class UserService
    {
        private void ValidateUserAndPasswordOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
            ValidatePasswordIsNullOrEmpty(password);

            Validate(
                (Rule: IsInvalidX(user.Id), Parameter: nameof(User.Id)),
                (Rule: IsInvalidX(user.FirstName), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalidX(user.LastName), Parameter: nameof(User.LastName)),
                (Rule: IsInvalidX(user.Email), Parameter: nameof(User.Email)),
                (Rule: IsInvalidX(user.CreatedDate), Parameter: nameof(User.CreatedDate)),
                (Rule: IsInvalidX(user.UpdatedDate), Parameter: nameof(User.UpdatedDate)),
                (Rule: IsInvalidX(password), Parameter: nameof(password))
                );
        }

        private static dynamic IsInvalidX(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required."
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Value can't be null, white space or empty."
        };

        private static dynamic IsInvalidX(DateTime dateTime) => new
        {
            Condition = dateTime == default,
            Message = "Date is required."
        };

        private static void ValidateUserIsNull(User user)
        {
            if (user is null)
                throw new NullUserException();
        }

        private static void ValidatePasswordIsNullOrEmpty(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new NullUserPasswordException();
        }


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserException =
                new InvalidUserException();

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
