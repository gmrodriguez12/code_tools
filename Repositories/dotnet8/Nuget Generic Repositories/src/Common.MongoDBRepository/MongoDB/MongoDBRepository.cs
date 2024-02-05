
using MongoDB.Driver;

namespace Common.MongoDBRepository.MongoDB
{
    public class MongoDBRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _dbCollection;
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoDBRepository(IMongoDatabase database, string collectionName)
        {
            _dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(x => x.Id, id);
            var existingEntity = await _dbCollection.Find(filter).FirstOrDefaultAsync();

            if (existingEntity != null)
            {
                await _dbCollection.DeleteOneAsync(filter);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid id");
            }

            var filter = _filterBuilder.Eq(x => x.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var filter = _filterBuilder.Eq(x => x.Id, entity.Id);
            return _dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
}