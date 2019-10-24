using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Morsley.UK.YearPlanner.Users.API.Swagger;
using Morsley.UK.YearPlanner.Users.Application.IoC;
using Morsley.UK.YearPlanner.Users.Infrastructure.IoC;
using Morsley.UK.YearPlanner.Users.Persistence.IoC;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Morsley.UK.YearPlanner.Users.API
{
    public class StartUp
    {
        private readonly IConfiguration _configuration;

        public StartUp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Dependency Injection...
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            AddApiVersioning(services);

            services.AddApplication();

            AddInfrastructure(services);

            AddPersistence(services);
        }

        private void AddInfrastructure(IServiceCollection services)
        {
            services.AddInfrastructure();
        }

        private static void AddPersistence(IServiceCollection services)
        {
            var settings = Shared.Environment.GetEnvironmentVariableValueByKey(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);

            if (string.IsNullOrEmpty(settings))
            {
                Log.Fatal("Could not determine Persistence Key! :-(");
                return;
            }

            services.AddPersistence(settings);
        }

        // Pipeline...
        public void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment hostingEnvironment,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseHttpsRedirection();

            applicationBuilder.UseRouting();

            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureApiVersioning(applicationBuilder, apiVersionDescriptionProvider);
        }

        #region Private Methods

        private void AddApiVersioning(IServiceCollection services)
        {
            services.Configure<OpenApiInfo>(_configuration.GetSection(nameof(OpenApiInfo)));

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        private void ConfigureApiVersioning(
            IApplicationBuilder applicationBuilder,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            applicationBuilder.UseSwagger();

            applicationBuilder.UseSwaggerUI(options =>
            {
                options.DefaultModelExpandDepth(0);
                options.DefaultModelsExpandDepth(0);
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });
        }

        #endregion Private Methods
    }
}