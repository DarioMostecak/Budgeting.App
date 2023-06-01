// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Services.Foundations.Accounts;
using Budgeting.App.Api.Services.Foundations.Users;

namespace Budgeting.App.Api.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService : IUserOrchestrationService
    {
        private readonly IUserService userService;
        private readonly IAccountService accountService;
        private readonly ILoggingBroker loggingBroker;

        public UserOrchestrationService(
            IUserService userService,
            IAccountService accountService,
            ILoggingBroker loggingBroker)
        {
            this.userService = userService;
            this.accountService = accountService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> RegirsterUserAsync(User user) =>
        TryCatch(async () =>
        {
            return user;
        });
    }
}
