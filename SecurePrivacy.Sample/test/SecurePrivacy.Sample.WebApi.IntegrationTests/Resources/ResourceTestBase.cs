using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SecurePrivacy.Sample.WebApi.IntegrationTests.Resources
{
    public abstract class ResourceTestBase<TFactory, TStartup> : IClassFixture<TFactory>, IAsyncLifetime where TFactory : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected const string MediaType = "application/json";

        protected readonly Encoding Encoding = Encoding.UTF8;

        protected TFactory Factory { get; private set; }

        protected HttpClient Client { get; set; }

        public ResourceTestBase(WebApplicationFactory<TStartup> factory)
        {
            Factory = (TFactory)factory;
            Client = Factory.CreateClient();
        }

        public abstract Task InitializeAsync();

        public abstract Task DisposeAsync();
    }
}
