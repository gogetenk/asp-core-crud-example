using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Helpers
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension of <see cref="OptionsConfigurationServiceCollectionExtensions.Configure{TOptions}(IServiceCollection,IConfiguration)"/>
        /// to auto-bind configuration section based on TOption class name
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureByConvention<TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
        {
            return services.Configure<TOptions>(config.GetSection(typeof(TOptions).Name));
        }

    }
}
