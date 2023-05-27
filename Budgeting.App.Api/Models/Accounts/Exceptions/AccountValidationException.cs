// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.App.Api.Models.Accounts.Exceptions
{
    public class AccountValidationException : ExceptionModel
    {
        public AccountValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data: data)
        { }
    }
}
