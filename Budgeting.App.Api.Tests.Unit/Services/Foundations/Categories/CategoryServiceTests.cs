// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.ExceptionModels;
using Budgeting.App.Api.Services.Foundations.Categories;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static Category CreateRandomCategory() =>
            CreateRandomCategoryFiller(dates: DateTime.UtcNow).Create();

        private static IQueryable<Category> CreateRandomCategories(DateTime dates) =>
            CreateRandomCategoryFiller(dates).Create(GetRandomNumber()).AsQueryable();

        private static DateTime GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetSafeUninitializedObject(typeof(MongoException));

        private static MongoWriteException GetMongoDuplicateKeyException() =>
            (MongoWriteException)FormatterServices.GetUninitializedObject(typeof(MongoWriteException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
            actualException.Message == expectedException.Message
            && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);

        }

        private static Filler<Category> CreateRandomCategoryFiller(DateTime dates)
        {
            var filler = new Filler<Category>();
            Guid categoryId = Guid.NewGuid();

            filler.Setup()
                .OnProperty(category => category.CategoryId).Use(categoryId)
                .OnProperty(category => category.Title).Use("Lunch")
                .OnProperty(category => category.Icon).Use("ffff")
                .OnProperty(category => category.TimeCreated).Use(dates)
                .OnProperty(category => category.TimeModify).Use(dates.AddDays(10));

            return filler;
        }

        private static IEnumerable<object[]> InvalidDataCategory() =>
            new List<object[]>
            {
                new object[] {Guid.Empty, "   ", DateTime.MinValue, DateTime.MinValue },
                new object[] {Guid.Empty, null, DateTime.MinValue, DateTime.MinValue },
                new object[] {Guid.Empty, new MnemonicString(1, 1, 1).GetValue(), DateTime.MinValue, DateTime.MinValue },
                new object[] {Guid.Empty, new MnemonicString(1, 20, 20).GetValue(), DateTime.MinValue, DateTime.MinValue }
            };
    }
}
