using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;
using Budgeting.App.Api.Models.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfMongoExceptionErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            MongoException mongoException = GetMongoException();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(mongoException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .Throws(mongoException);

            //when
            ValueTask<CategoryDto> removeCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(someId);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                removeCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryDependencyException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.DeleteCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .Throws(serviceException);

            //when
            ValueTask<CategoryDto> removeCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(someId);

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                removeCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryServiceException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.DeleteCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
