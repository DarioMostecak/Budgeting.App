using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Services.Foundations.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userService = new UserService(
                apiBroker: this.apiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<ExceptionModel, bool>> SameExceptionAs(ExceptionModel expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);
        }

        private static TheoryData DependencyApiExceptions()
        {
            string exceptionMessage = "Request fail.";
            var responseMessage = new HttpResponseMessage();

            var httpRequestException =
                new HttpRequestException();

            var httpResponseInternalServerErrorException =
                new HttpResponseInternalServerErrorException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseException =
                new HttpResponseException(
                    responseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpRequestException,
                httpResponseInternalServerErrorException,
                httpResponseException
            };
        }

        private static string CreatePassword() =>
            "12345678";


        private static User CreateUser() =>
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Test1",
                Email = "Test@mail.com",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(10),
            };

        private static IEnumerable<object[]> InvalidUserData() =>
            new List<object[]>
            {
                new object[]{ Guid.Empty ,null, null, null, DateTime.MinValue, DateTime.MinValue, " " },
                new object[]{ Guid.Empty ,string.Empty, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, "  " },
                new object[]{ Guid.Empty ,"   ", "   ", "   ", DateTime.MinValue, DateTime.MinValue, "  " }
            };
    }
}
