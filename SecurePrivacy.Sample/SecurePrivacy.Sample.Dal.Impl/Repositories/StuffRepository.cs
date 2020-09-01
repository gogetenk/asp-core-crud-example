using System.Threading.Tasks;
using AutoMapper;
using Dal.Impl.Configurations;
using Dal.Impl.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SecurePrivacy.Sample.Dal.Exceptions;
using SecurePrivacy.Sample.Dal.Repositories;
using SecurePrivacy.Sample.Model;

namespace Dal.Impl.Repositories
{
    public class StuffRepository : RepositoryBase, IStuffRepository
    {
        private readonly IMapper _mapper;
        private readonly IClientSessionHandle _session;
        private readonly DatabaseConfiguration _config;
        protected override string CollectionName => _config.StuffCollectionName;

        public StuffRepository(IMongoDbContext mongoDbContext, ILogger<RepositoryBase> logger, IMapper mapper, IOptionsSnapshot<DatabaseConfiguration> config, IClientSessionHandle session)
            : base(mongoDbContext, logger, mapper)
        {
            _mapper = mapper;
            _session = session;
            _config = config.Value;
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Stuff> Insert(Stuff entity)
        {
            var collection = GetCollection<StuffEntity>();
            var dbEntity = _mapper.Map<StuffEntity>(entity);
            await collection.InsertOneAsync(_session, dbEntity);
            return _mapper.Map<Stuff>(dbEntity);
        }

        /// <summary>
        /// Get an entity from its id
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Stuff> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                throw new DalException("The object id is not well formated");

            var collection = GetCollection<StuffEntity>();
            var results = await collection
                .AsQueryable()
                .Where(x => x.Id == objectId)
                .FirstOrDefaultAsync();

            return _mapper.Map<Stuff>(results);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> Update(string id, Stuff entity)
        {
            var dbEntity = _mapper.Map<StuffEntity>(entity);
            var collection = GetCollection<StuffEntity>();
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                throw new DalException("The object id is not well formated");

            dbEntity.Id = objectId;
            var result = await collection.ReplaceOneAsync<StuffEntity>(_session, x => x.Id == objectId, dbEntity);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entityId"></param>
        public async Task Delete(string entityId)
        {
            var collection = GetCollection<StuffEntity>();
            var builder = Builders<StuffEntity>.Filter;
            var filter = builder.Eq(x => x.Id, new ObjectId(entityId));

            await collection.DeleteOneAsync(_session, filter);
        }

    }
}
