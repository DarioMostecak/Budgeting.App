using Budgeting.Web.App.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Budgeting.Web.App.Brokers.Storages
{
    public partial class StorageBroker
    {
        private IMongoCollection<Category> categories;

        //see if is better to return mongodb results from update, delete
        public async ValueTask<Category> InsertCategoryAsync(Category category)
        {
            var broker = new StorageBroker(this.configuration);
            this.categories = broker.db
                .GetCollection<Category>(GetCollectionName<Category>());

            await this.categories.InsertOneAsync(category);
            return category;
        }

        public IQueryable<Category> SelectAllCategories()
        {
            var broker = new StorageBroker(this.configuration);
            this.categories = broker.db
                .GetCollection<Category>(GetCollectionName<Category>());

            return this.categories.AsQueryable();
        }

        public async ValueTask<Category> SelectCategoriesByIdAsync(Guid categoryId)
        {
            var broker = new StorageBroker(this.configuration);
            this.categories = broker.db
                .GetCollection<Category>(GetCollectionName<Category>());

            return await this.categories
                .Find(category => category.CategoryId == categoryId)
                .FirstOrDefaultAsync();
        }

        public async ValueTask<Category> UpdateCategoryAsync(Category category)
        {
            var broker = new StorageBroker(this.configuration);
            this.categories = broker.db
                .GetCollection<Category>(GetCollectionName<Category>());

            await this.categories
                .ReplaceOneAsync(_category => _category.CategoryId == category.CategoryId, category);

            return category;
        }

        public async ValueTask<Category> DeleteCategoryAsync(Category category)
        {
            var broker = new StorageBroker(this.configuration);
            this.categories = broker.db
                .GetCollection<Category>(GetCollectionName<Category>());

            await this.categories.DeleteOneAsync(_category => _category.CategoryId == category.CategoryId);

            return category;
        }
    }
}
