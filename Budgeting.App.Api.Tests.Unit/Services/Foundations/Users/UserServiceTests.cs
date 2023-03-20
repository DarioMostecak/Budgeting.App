using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagment;
using Budgeting.App.Api.Models.ExceptionModels;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Services.Foundations.Users;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IUserManagerBroker> userManagerBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.userManagerBrokerMock = new Mock<IUserManagerBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userService = new UserService(
                userManagerBroker: this.userManagerBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTime GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetSafeUninitializedObject(typeof(MongoException));

        private static MongoWriteException GetMongoWriteException() =>
            (MongoWriteException)FormatterServices.GetUninitializedObject(typeof(MongoWriteException));

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();

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

        private static User CreateUser()
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

        private static IEnumerable<object[]> InvalidDataUser() =>
            new List<object[]>
            {
                new object[] {Guid.Empty, null, null, null, DateTime.MinValue, DateTime.MinValue, new MnemonicString(1, 7, 7).GetValue() },
                new object[] {Guid.Empty, "  ", "  ", "  ", DateTime.MinValue, DateTime.MinValue, "dfxcxd waaaf" },

                new object[] {Guid.Empty, new MnemonicString(1, 2, 2).GetValue(), new MnemonicString(1, 2, 2).GetValue(), new MnemonicString(1, 2, 2).GetValue(),
                              DateTime.MinValue, DateTime.MinValue,  new MnemonicString(1, 26, 26).GetValue()},

                new object[] {Guid.Empty, new MnemonicString(1, 21, 21).GetValue(), new MnemonicString(1, 21, 21).GetValue(), new MnemonicString(1, 21, 21).GetValue(),
                              DateTime.MinValue, DateTime.MinValue,  new MnemonicString(1, 26, 26).GetValue()}
            };
    }
}
