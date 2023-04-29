using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Services.Foundations.Users;
using Moq;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.userService = new UserService(
                apiBroker: this.apiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static Expression<Func<ExceptionModel, bool>> SameExceptionAs(ExceptionModel expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);
        }

        private static TheoryData DependencyApiExceptions()
        {
            string exceptionMessage = "Request fail";
            var responseMessage = new HttpResponseMessage();

            var httpRequestException =
                new HttpRequestException();

            var httpRequestNotFound =
                new HttpResponseNotFoundException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

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
                httpRequestNotFound,
                httpResponseBadRequestException,
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
    }
}
