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
