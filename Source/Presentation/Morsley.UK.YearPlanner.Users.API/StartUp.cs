using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Morsley.UK.YearPlanner.Users.API.Swagger;
using Morsley.UK.YearPlanner.Users.Application.IoC;
using Morsley.UK.YearPlanner.Users.Infrastructure.IoC;
using Morsley.UK.YearPlanner.Users.Persistence.IoC;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

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
            AddControllers(services);

            AddApiVersioning(services);

            services.AddApplication();

            //var executingAssembly = Assembly.GetExecutingAssembly();
            //services.AddAutoMapper(executingAssembly);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(assemblies);

            AddInfrastructure(services);

            AddPersistence(services);
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

            //applicationBuilder.UseAuthorization();

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
                options.IncludeXmlComments(XmlCommentsFilePath);
            });

            services.AddApiVersioning(options =>
            {
                //options.AssumeDefaultVersionWhenUnspecified = true;
                //options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            //services.ConfigureSwaggerGen(options =>
            //{
            //    options.CustomSchemaIds(x => x.FullName);
            //});
        }

        private static void AddControllers(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(setupAction =>
                    {
                        setupAction.SerializerSettings
                                   .ContractResolver = new CamelCasePropertyNamesContractResolver();
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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

        private void ConfigureApiVersioning(
            IApplicationBuilder applicationBuilder,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            applicationBuilder.UseSwagger();

            applicationBuilder.UseSwaggerUI(options =>
            {
                //options.DefaultModelExpandDepth(0);
                //options.DefaultModelsExpandDepth(0);

                // Build a Swagger endpoint for each discovered API version...
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = "";
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(StartUp).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
        
        #endregion Private Methods
    }
}