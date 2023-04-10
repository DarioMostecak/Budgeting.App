using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagment;
using Budgeting.App.Api.Models.ExceptionModels;
using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Options;
using Budgeting.App.Api.Services.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
    {
        private readonly Mock<IUserManagerBroker> userManagerBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IOptions<JwtSettings>> jwtOptionsMock;
        private readonly IIdentityService identityService;


        public IdentityServiceTests()
        {
            this.userManagerBrokerMock = new Mock<IUserManagerBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
            this.jwtOptionsMock.Setup(jwt => jwt.Value).Returns(GetJwtSettings());

            this.identityService = new IdentityService(
                userManagerBroker: this.userManagerBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                jwtOptions: this.jwtOptionsMock.Object);
        }

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetUninitializedObject(typeof(MongoException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
            actualException.Message == expectedException.Message
            && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);

        }

        private IdentityRequest CreateRandomIdentityRequest() =>
            new IdentityRequest
            {
                Email = "person@mail.com",
                Password = "22222222"
            };

        private static User CreateRandomUser()
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "Travis@mail.com",
                FirstName = "Travis",
                LastName = "Kongo",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow.AddDays(10),
            };

            return newUser;
        }

        private static IEnumerable<object[]> InvalidIdentityRequestData() =>
            new List<object[]>
            {
                new object[] { null, null },
                new object[] {" ", " "},
                new object[] {"xcsdfascas", null}
            };

        private static JwtSettings GetJwtSettings() =>
            new JwtSettings
            {
                SigningKey = "f5422e6cdfde4af3bf631c7dd1f80b97",
                Audiences = new string[] { "SwaggerUI" },
                Issuer = "BudgetingAppApi"
            };

    }
}
