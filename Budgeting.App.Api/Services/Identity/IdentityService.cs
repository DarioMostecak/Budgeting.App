// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagers;
using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Options;
using Microsoft.Extensions.Options;

namespace Budgeting.App.Api.Services.Identity
{
    public partial class IdentityService : IIdentityService
    {
        public readonly IUserManagerBroker userManagerBroker;
        public readonly ILoggingBroker loggingBroker;
        private readonly JwtSettings jwtSettings;

        public IdentityService(
            IUserManagerBroker userManagerBroker,
            ILoggingBroker loggingBroker,
            IOptions<JwtSettings> jwtOptions)
        {
            this.userManagerBroker = userManagerBroker;
            this.loggingBroker = loggingBroker;
            this.jwtSettings = jwtOptions.Value;
        }

        public ValueTask<IdentityResponse> AuthenticateUserAsync(IdentityRequest identityRequest) =>
        TryCatch(async () =>
        {
            ValidateIdentityRequest(identityRequest);

            User maybeUser =
                await this.userManagerBroker.SelectUserByEmailAsync(identityRequest.Email);

            ValidateIdentityIsNull(
                maybeUser,
                identityRequest.Email);

            bool confirmPassword =
                await this.userManagerBroker.ConfirmUserByPasswordAsync(
                    maybeUser,
                    identityRequest.Password);

            ValidateIdentityPasswordConfirmationIsFalse(
                confirmPassword,
                identityRequest.Password);

            IdentityResponse identityResponse =
                 CreateIdentityResponse(maybeUser);

            return identityResponse;
        });

    }
}
