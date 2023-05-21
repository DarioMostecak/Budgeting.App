using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.Categories.Exceptions;
using Budgeting.Web.App.Models.ExceptionModels;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionOnAddIfUnauthorizedExceptionOccursAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            var exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseUnauthorizeException =
                new HttpResponseUnauthorizedException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var failedCategoryUnauthorizedException =
                new FailedCategoryUnauthorizedException(httpResponseUnauthorizeException);

            var expectedCategoryUnauthorizedException =
                new CategoryUnauthorizedException(failedCategoryUnauthorizedException);
            this.apiBrokerMock.Setup(broker =>
                broker.PostCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(httpResponseUnauthorizeException);

            //when
            ValueTask<Category> addCategoryTask =
                this.categoryServiceMock.AddCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryUnauthorizedException>(() =>
                addCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryUnauthorizedException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyApiException))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccurresAndLogItAsync(
            Exception dependencyValidationException)
        {
            //given
            Category randomCategory = CreateRandomCategory();

            var failedCategoryDependencyException =
                new FailedCategoryDependencyException(dependencyValidationException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(dependencyValidationException);

            //when
            ValueTask<Category> addCategoryTask =
                this.categoryServiceMock.AddCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                addCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
