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
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionOnRetrieveAllIfUnauthorizedExceptionOccursAndLogItAsync()
        {
            //given
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
                broker.GetCategoriesAsync())
                        .ThrowsAsync(httpResponseUnauthorizeException);

            //when
            ValueTask<IEnumerable<Category>> retrieveAllCategoriesTask =
                this.categoryServiceMock.RetrieveAllCategoriesAsync();

            //then
            await Assert.ThrowsAsync<CategoryUnauthorizedException>(() =>
                retrieveAllCategoriesTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoriesAsync(),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCategoryUnauthorizedException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoulThrowDependencyExceptionOnRetrieveAllAsyncIfHttpRequestExceptionOccursAndLogItAsync()
        {
            //given
            var httpRequestException = new HttpRequestException();

            var failedCategoryDependencyEception =
                new FailedCategoryDependencyException(httpRequestException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryDependencyEception);

            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoriesAsync())
                        .ThrowsAsync(httpRequestException);

            //when
            ValueTask<IEnumerable<Category>> retrieveAllCategoriesTask =
                this.categoryServiceMock.RetrieveAllCategoriesAsync();

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                retrieveAllCategoriesTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoriesAsync(),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedCategoryDependencyException))),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfHttpResponseExceptionOccursAndLogItAsync()
        {
            //given
            var exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseException =
                new HttpResponseException(
                    httpResponseMessage: responseMessage,
                    message: exceptionMessage);

            var failedCategoryDependencyException =
                new FailedCategoryDependencyException(httpResponseException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoriesAsync())
                        .ThrowsAsync(httpResponseException);

            //when
            ValueTask<IEnumerable<Category>> retrieveAllCategoriesTask =
                this.categoryServiceMock.RetrieveAllCategoriesAsync();

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                retrieveAllCategoriesTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoriesAsync(),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedCategoryDependencyException))),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllAsyncIfExceptionOccursAndLogItAsync()
        {
            //given
            var serviceException = new Exception();

            var failedCategoryserviceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryserviceException);

            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoriesAsync())
                        .ThrowsAsync(serviceException);

            //when
            ValueTask<IEnumerable<Category>> retrieveAllCategoriesTask =
                this.categoryServiceMock.RetrieveAllCategoriesAsync();

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                retrieveAllCategoriesTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoriesAsync(),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedCategoryServiceException))),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }

}
