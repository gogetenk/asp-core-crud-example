using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MongoDB.Driver;
using Newtonsoft.Json;
using SecurePrivacy.Sample.Dto;
using SecurePrivacy.Sample.WebApi.IntegrationTests.Factory;
using Xunit;

namespace SecurePrivacy.Sample.WebApi.IntegrationTests.Resources
{
    public class StuffResourceTest : ResourceTestBase<IntegrationTestsApplicationFactory, Startup>
    {
        public const string _stuffEndpoint = "stuff";
        private static MongoClient _mongoClient;

        public StuffResourceTest(IntegrationTestsApplicationFactory factory) : base(factory)
        {
            // We instantiate the mongo client only once for all the tests
            if (_mongoClient is null)
                _mongoClient = new MongoClient(Factory.Configuration["DatabaseConfiguration:ConnectionString"]);
        }

        #region Lifecycle

        public override async Task DisposeAsync()
        {
            // Deleting test collection to keep data consistency
            var nexecurDb = _mongoClient.GetDatabase(Factory.Configuration["DatabaseConfiguration:DatabaseName"]);
            await nexecurDb.DropCollectionAsync(Factory.CollectionName);
        }

        public override async Task InitializeAsync()
        {
            // Generating a guid for collection names so it will be easier to clean testing data
            Factory.CollectionName = Guid.NewGuid().ToString();
            await Task.CompletedTask;
        }

        #endregion

        [Fact]
        public async Task Post_NominalCase_Expect201Created()
        {
            // Arrange
            var uri = $"api/{_stuffEndpoint}";
            var stuffFixture = new Fixture().Create<StuffDto>();
            var payload = new StringContent(JsonConvert.SerializeObject(stuffFixture), Encoding, MediaType);

            // Act
            var response = await Client.PostAsync(uri, payload);
            var json = await response.Content.ReadAsStringAsync();
            var respObject = JsonConvert.DeserializeObject<StuffDto>(json);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            respObject.Should().NotBeNull();
        }
    }
}
