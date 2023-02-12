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
        public async Task ShouldThrowDependencyValidationExceptionOnCreateWhenCategoryAlredyExistLogItAasync()
        {
            //given
            CategoryDto someCategoryDto = CreateRandomCategoryDto();
            MongoDuplicateKeyException mongoDuplicateKeyException =
                GetMongoDuplicateKeyException();

            var alreadyExistsCategoryException =
                new AlreadyExistsCategoryException(mongoDuplicateKeyException);

            var expectedCategoryValidationException =
                new CategoryValidationException(alreadyExistsCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCategoryAsync(It.IsAny<Category>()))
                  .Throws(mongoDuplicateKeyException);


            //when
            ValueTask<CategoryDto> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategoryDto);

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
        public async Task ShouldThrowDependencyExceptionOnCreateIfMongoExceptionErrorOccursAndLogItAsync()
        {
            //given
            CategoryDto someCategoryDto = CreateRandomCategoryDto();
            MongoException mongoException = GetMongoException();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(mongoException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .Throws(mongoException);

            //when
            ValueTask<CategoryDto> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategoryDto);

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
        public async Task ShouldThrowServiceExceptionOnCreateIfExceptionOccursAndLogItAsync()
        {
            //given
            CategoryDto someCategoryDto = CreateRandomCategoryDto();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .Throws(serviceException);

            //when
            ValueTask<CategoryDto> addCategoryTask =
                this.categoryService.AddCategoryAsync(someCategoryDto);

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
