using Budgeting.App.Api.Models.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfMongoExceptionErrorOccursAndLogItAsync()
        {
            //given
            MongoException mongoException = GetMongoException();

            var expectedCategoryDependencyException =
                new CategoryDependencyException(mongoException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllCategories())
                .Throws(mongoException);

            //when . then
            Assert.Throws<CategoryDependencyException>(() =>
                this.categoryService.RetrieveAllCategories());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryDependencyException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectAllCategories(),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfExceptionOccursAndLogItAsync()
        {
            //given
            Exception serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllCategories())
                .Throws(serviceException);

            //when . then
            Assert.Throws<CategoryServiceException>(() =>
                this.categoryService.RetrieveAllCategories());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryServiceException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectAllCategories(),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

