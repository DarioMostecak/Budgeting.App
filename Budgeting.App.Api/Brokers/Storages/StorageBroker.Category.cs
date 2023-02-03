using Budgeting.App.Api.Models;
using MongoDB.Driver;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private IMongoCollection<Category> categoryCollection;


        public async ValueTask<Category> InsertCategoryAsync(Category category)
        {
            this.categoryCollection =
                this.db.GetCollection<Category>(GetCollectionName<Category>());

            await this.categoryCollection.InsertOneAsync(category);

            return category;
        }

        public IQueryable<Category> SelectAllCategories()
        {
            this.categoryCollection =
                this.db.GetCollection<Category>(GetCollectionName<Category>());

            return this.categoryCollection.AsQueryable();
        }

        public async ValueTask<Category> SelectCategoriesByIdAsync(Guid categoryId)
        {
            this.categoryCollection =
                this.db.GetCollection<Category>(GetCollectionName<Category>());

            var categroy = await this.categoryCollection.Find(obj => obj.CategoryId == categoryId).FirstOrDefaultAsync();

            return categroy;
        }

        public async ValueTask<Category> UpdateCategoryAsync(Category category)
        {
            this.categoryCollection =
                this.db.GetCollection<Category>(GetCollectionName<Category>());

            await this.categoryCollection.ReplaceOneAsync(obj => obj.CategoryId == category.CategoryId, category);

            return category;
        }

        public async ValueTask<Category> DeleteCategoryAsync(Category category)
        {
            this.categoryCollection =
                this.db.GetCollection<Category>(GetCollectionName<Category>());

            await this.categoryCollection.DeleteOneAsync(obj => obj.CategoryId == category.CategoryId);

            return category;
        }
    }
}
