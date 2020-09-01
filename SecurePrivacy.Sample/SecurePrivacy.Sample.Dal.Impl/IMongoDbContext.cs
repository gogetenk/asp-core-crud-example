using MongoDB.Driver;

namespace Dal.Impl
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase(string databaseName = null);
    }
}
