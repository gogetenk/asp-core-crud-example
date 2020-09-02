using Dal.Impl.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecurePrivacy.Sample.WebApi.IntegrationTests.Factory
{
    /// <summary>
    /// Startup class for IT
    /// </summary>
    public class IntegrationTestsApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Environment Name used to disable swagger
        /// </summary>
        private const string _IntegrationTestsEnvironment = "IntegrationTests";

        /// <summary>
        /// Name of the temporary collection used by the tests
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets configuration
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gives a fixture an opportunity to configure the application before it gets built.
        /// </summary>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("./src/SecurePrivacy.Sample.WebApi/") // Needed to make the test to work
                .UseEnvironment(_IntegrationTestsEnvironment) // Needed in order to disable Swagger Doc (fix for Live Unit Test to work)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    Configuration = config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .Build();
                })
                .ConfigureTestServices(services =>
                {
                    services.PostConfigure<DatabaseConfiguration>((config) => config.StuffCollectionName = CollectionName); // We configure a specific collection name for the Integration Tests
                })
                .UseStartup<Startup>()
                .UseEnvironment(_IntegrationTestsEnvironment);
        }
    }
}
