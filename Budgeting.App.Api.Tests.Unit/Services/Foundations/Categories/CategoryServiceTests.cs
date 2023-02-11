using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Services.Foundations.Categories;
using MongoDB.Driver;
using Moq;
using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICategoryService categoryService;

        public CategoryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.categoryService = new CategoryService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static CategoryDto CreateRandomCategoryDto() =>
            CreateRandomCategoryDtoFiller(dates: DateTime.UtcNow).Create();

        //private static string CreateRandomMessage() => new MnemonicString().GetValue();

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetSafeUninitializedObject(typeof(MongoException));

        private static MongoDuplicateKeyException GetMongoDuplicateKeyException() =>
            (MongoDuplicateKeyException)FormatterServices.GetUninitializedObject(typeof(MongoDuplicateKeyException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {

            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Filler<CategoryDto> CreateRandomCategoryDtoFiller(DateTime dates)
        {
            var filler = new Filler<CategoryDto>();
            Guid categoryId = Guid.NewGuid();

            filler.Setup()
                .OnProperty(category => category.CategoryId).Use(categoryId)
                .OnProperty(category => category.Title).Use("Lunch")
                .OnProperty(category => category.Icon).Use("ffff")
                .OnProperty(category => category.Type).Use("Expense")
                .OnProperty(category => category.TimeCreated).Use(dates)
                .OnProperty(category => category.TimeModify).Use(dates.AddDays(10));

            return filler;
        }
    }
}
