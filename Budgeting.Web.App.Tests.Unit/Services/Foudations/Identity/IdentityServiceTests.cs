using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Services.Foundations;
using Budgeting.Web.App.Services.Foundations.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Identity
{
    public partial class IdentityServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IIdentityService identityService;

        public IdentityServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.identityService = new IdentityService(
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
            string exceptionMessage = "Request fail";
            var responseMessage = new HttpResponseMessage();

            var httpRequestException =
                new HttpRequestException();

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
                httpResponseBadRequestException,
                httpResponseInternalServerErrorException,
                httpResponseException
            };
        }

        private AuthenticationRequest CreateAuthenticationRequest() =>
            new AuthenticationRequest
            {
                Email = "dario@gmil.com",
                Password = "11111111"
            };

        private AuthenticationResult CreateAuthenticationResult() =>
            new AuthenticationResult
            {
                Token = "dlkefd0300mdjvndf56464djjhu"
            };

        private static IEnumerable<object[]> InvalidDataAuthenticationRequest() =>
            new List<object[]>
            {
                new object[] {string.Empty, string.Empty },
                new object[] {null, null },
                new object[] {"  ", "  "}
            };
    }
}
