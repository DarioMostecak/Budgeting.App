// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;

namespace Budgeting.App.Api.Services.Foundations.Accounts
{
    public partial class AccountService
    {
        private static void ValidateAccountOnCreate(Account account)
        {
            ValidateAccountIsNull(account);

            Validate(
                (rule: IsInvalidX(account.AccountId), parameter: nameof(Account.AccountId)),
                (rule: IsInvalidX(account.UserIdentityId), parameter: nameof(Account.UserIdentityId)),
                (rule: IsInvalidX(account.Balance), parameter: nameof(Account.Balance)),
                (rule: IsInvalidX(account.TimeCreated), parameter: nameof(Account.TimeCreated)),
                (rule: IsInvalidX(account.TimeModify), parameter: nameof(Account.TimeModify))
                );
        }

        private static dynamic IsInvalidX(Guid accountId) => new
        {
            Condition = accountId == Guid.Empty,
            Message = "Id isn't valid.",
        };

        private static dynamic IsInvalidX(string userIdentityId) => new
        {
            Condition = !Guid.TryParse(userIdentityId, out _)
               || string.IsNullOrWhiteSpace(userIdentityId),

            Message = "UserIdentityId isn't valid.",
        };

        private static dynamic IsInvalidX(decimal decimalNumber) => new
        {
            Condition = decimalNumber != 0,
            Message = "Balance must be zero.",
        };

        private static dynamic IsInvalidX(DateTime date) => new
        {
            Condition = date == default,
            Message = "Date is required."
        };

        private static void ValidateUserIdentityIdIsInvalid(string userIdentityId)
        {
            if (!Guid.TryParse(userIdentityId, out _) || string.IsNullOrWhiteSpace(userIdentityId))
                throw new InvalidAccountException(
                    parameterName: nameof(userIdentityId),
                    parameterValue: userIdentityId);
        }

        private static void ValidateStorageAccountIsNull(Account storageAccount, string userIdentityId)
        {
            if (storageAccount is null)
                throw new NotFoundAccountException(userIdentityId);
        }

        private static void ValidateAccountIsNull(Account account)
        {
            if (account is null)
                throw new NullAccountException();
        }

        private static void Validate(params (dynamic rule, string parameter)[] validations)
        {
            var invalidaccountException = new InvalidAccountException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidaccountException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidaccountException.ThrowIfContainsErrors();
        }
    }
}
