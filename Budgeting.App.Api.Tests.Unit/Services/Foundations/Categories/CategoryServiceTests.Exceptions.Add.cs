using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnAddWhenCategoryAlredyExistLogItAasync()
        {
            //given
            Category someCategory = CreateRandomCategory();
            MongoWriteException mongoDuplicateKeyException =
                GetMongoDuplicateKeyException();

            var alreadyExistsCategoryException =
                new AlreadyExistsCategoryException(mongoDuplicateKeyException);

            var expectedCategoryValidationException =
                new CategoryValidationException(alreadyExistsCategoryException, alreadyExistsCategoryException.Data);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()))
                  .ThrowsAsync(mongoDuplicateKeyException);


            //when
            ValueTask<Category> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
               addCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryValidationException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfMongoExceptionErrorOccursAndLogItAsync()
        {
            //given
            Category someCategory = CreateRandomCategory();
            MongoException mongoException = GetMongoException();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(mongoException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .ThrowsAsync(mongoException);

            //when
            ValueTask<Category> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategory);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                addCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryDependencyException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfExceptionOccursAndLogItAsync()
        {
            //given
            Category someCategory = CreateRandomCategory();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .ThrowsAsync(serviceException);

            //when
            ValueTask<Category> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategory);

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                addCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryServiceException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
