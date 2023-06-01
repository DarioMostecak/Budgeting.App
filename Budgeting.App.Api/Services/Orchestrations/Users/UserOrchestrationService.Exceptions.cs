// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Users;

namespace Budgeting.App.Api.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private delegate ValueTask<User> ReturnigUserFunction();

        private async ValueTask<User> TryCatch(
            ReturnigUserFunction returnigUserFunction)
        {
            try
            {
                return await returnigUserFunction();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
