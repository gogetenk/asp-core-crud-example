using AutoMapper;
using Dal.Impl;
using Dal.Impl.Configurations;
using Dal.Impl.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SecurePrivacy.Sample.Bll.Impl.services;
using SecurePrivacy.Sample.Bll.Services;
using SecurePrivacy.Sample.Dal.Impl;
using SecurePrivacy.Sample.Dal.Repositories;
using Serilog;
using WebApi.Filters;
using WebApi.Helpers;

namespace SecurePrivacy.Sample.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .ConfigureByConvention<DatabaseConfiguration>(Configuration);

            services.AddControllers(opts =>
            {
                opts.Filters.Clear();
                opts.Filters.Add<ExceptionFilter>();
                opts.Filters.Add<TransactionFilter>();
                opts.EnableEndpointRouting = true;
            });
            services.AddSwaggerGen(c =>
            {
                if (Env.EnvironmentName != "IntegrationTests")
                    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Stuff API", Version = "v1" });
            });
            services.AddAutoMapper(
                typeof(Startup).Assembly,
                typeof(StuffService).Assembly,
                typeof(DatabaseConfiguration).Assembly);

            // BLL
            services.TryAddTransient<IStuffService, StuffService>();

            // DAL
            services.TryAddTransient<IStuffRepository, StuffRepository>();
            AddTransactions(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Stuff API v1");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Adds a httprequest-wide transactions on all mongodb repositories.
        /// </summary>
        private void AddTransactions(IServiceCollection services)
        {
            services.TryAddSingleton<IMongoClient>(provider =>
            new MongoClient(provider.GetRequiredService<IOptionsMonitor<DatabaseConfiguration>>().CurrentValue.ConnectionString));
            services.TryAddSingleton<IMongoDbContext>(provider => new DefaultMongoDbContext(provider.GetRequiredService<IOptionsMonitor<DatabaseConfiguration>>().CurrentValue));
            // The session has to be scoped so we get a new one each http request
            services.TryAddScoped<IClientSessionHandle>(provider => provider.GetRequiredService<IMongoClient>().StartSession());
        }
    }
}
