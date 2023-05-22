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
        public async Task ShouldThrowUnauthorizedExceptionOnModifyIfUnauthorizedExceptionOccurresAndLogItAsync()
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
                broker.PutCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(httpResponseUnauthorizeException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryUnauthorizedException>(() =>
                modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryUnauthorizedException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionModifyAddIfHttpResponseNotFoundExceptionOccurresAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseNotFoundException =
                new HttpResponseNotFoundException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var failedCategoryDependencyException =
                new FailedCategoryDependencyException(httpResponseNotFoundException);

            var expectedCategoryDependencyValidationException =
                new CategoryDependencyValidationException(failedCategoryDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(httpResponseNotFoundException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfHttpResponseBadRequestExceptionOccurresAndLogitAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var invalidCategoryException =
                new InvalidCategoryException(httpResponseBadRequestException);

            var expectedCategoryDependencyValidationException =
                new CategoryDependencyValidationException(invalidCategoryException);

            this.apiBrokerMock.Setup(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(httpResponseBadRequestException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfHttpResponseConflictExceptionOccurresAndLogitAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var alreadyExistsCategoryException =
                new AlreadyExistsCategoryException(httpResponseConflictException);

            var expectedCategoryDependencyValidationException =
                new CategoryDependencyValidationException(alreadyExistsCategoryException);

            this.apiBrokerMock.Setup(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(httpResponseConflictException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyApiException))]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccurresAndLogItAsync(
            Exception dependencyValidationException)
        {
            //given
            Category randomCategory = CreateRandomCategory();

            var failedCategoryDependencyException =
                new FailedCategoryDependencyException(dependencyValidationException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()))
                        .ThrowsAsync(dependencyValidationException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfExceptionOccurresAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.apiBrokerMock.Setup(broker =>
               broker.PutCategoryAsync(It.IsAny<Category>()))
                       .ThrowsAsync(serviceException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                modifyCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryServiceException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
