using Dal.Impl;
using Dal.Impl.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace SecurePrivacy.Sample.Dal.Impl
{
    public class DefaultMongoDbContext : IMongoDbContext
    {
        private readonly string _databaseName;

        protected MongoClient MongoClient { get; set; }

        public DefaultMongoDbContext(DatabaseConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString;
            _databaseName = configuration.DatabaseName;

            RegisterConventions();
            MongoClient = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase(string databaseName = null)
        {
            var database = MongoClient.GetDatabase(databaseName ?? _databaseName);
            return database;
        }

        protected void RegisterConventions()
        {
            ConventionRegistry.Register(
                "IgnoreNullValues",
                new ConventionPack
                {
                    new IgnoreIfNullConvention(true)
                },
                t => true);
            ConventionRegistry.Register(
                "CamelCaseElementName",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                t => true);
            ConventionRegistry.Register(
                "EnumAsString",
                new ConventionPack
                {
                    new EnumRepresentationConvention(BsonType.String)
                },
                t => true);
            ConventionRegistry.Register(
                "IgnoreExtraElements",
                new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true)
                },
                t => true);
        }
    }

}
