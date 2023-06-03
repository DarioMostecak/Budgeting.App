// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Users;

namespace Budgeting.App.Api.Services.Orchestrations.Users
{
    public interface IUserOrchestrationService
    {
        ValueTask<User> RegisterUserAsync(User user, string password);
    }
}
