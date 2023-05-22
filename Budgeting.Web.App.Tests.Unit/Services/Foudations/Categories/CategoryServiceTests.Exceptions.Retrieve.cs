// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async Task ShouldThrowUnauthorizedExceptionOnRetrieveIfUnauthorizedExceptionOccurresAndLogItAsync()
        {
            //given
            Guid randomCategoryId = GetRandomId();
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
                broker.GetCategoryAsync(It.IsAny<string>()))
                        .ThrowsAsync(httpResponseUnauthorizeException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryUnauthorizedException>(() =>
                retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryUnauthorizedException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfHttpResponseNotFoundExceptionOccurresAndLogItAsync()
        {
            //given
            Guid randomCategoryId = GetRandomId();
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
                broker.GetCategoryAsync(It.IsAny<string>()))
                        .ThrowsAsync(httpResponseNotFoundException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfHttpResponseBadRequestExceptionOccurresAndLogitAsync()
        {
            //given
            Guid randomCategoryId = GetRandomId();
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
                broker.GetCategoryAsync(It.IsAny<string>()))
                        .ThrowsAsync(httpResponseBadRequestException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveIfHttpResponseConflictExceptionOccurresAndLogitAsync()
        {
            //given
            Guid randomCategoryId = GetRandomId();
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
                broker.GetCategoryAsync(It.IsAny<string>()))
                        .ThrowsAsync(httpResponseConflictException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryDependencyValidationException>(() =>
                 retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfDependencyErrorOccurresAndLogItAsync(
            Exception dependencyValidationException)
        {
            //given
            Guid randomCategoryId = GetRandomId();

            var failedCategoryDependencyException =
                new FailedCategoryDependencyException(dependencyValidationException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()))
                        .ThrowsAsync(dependencyValidationException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryDependencyException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfExceptionOccurresAndLogItAsync()
        {
            //given
            Guid randomCategoryId = GetRandomId();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.apiBrokerMock.Setup(broker =>
               broker.GetCategoryAsync(It.IsAny<string>()))
                       .ThrowsAsync(serviceException);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(randomCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                retrieveCategoryTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
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
