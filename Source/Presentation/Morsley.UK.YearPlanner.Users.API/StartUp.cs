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

            services.Configure<OpenApiInfo>(_configuration.GetSection(nameof(OpenApiInfo)));

            //var openApiInfo = services.GetType();

            //services.AddSingleton<IC>()

            AddApiVersioning(services);
        }

        //private static void AddApiExplorerOptions(ApiExplorerOptions options)
        //{
        //    options.SubstituteApiVersionInUrl = true;
        //}

        //private static void AddApiVersioningOptions(ApiVersioningOptions options)
        //{
        //    options.ReportApiVersions = true;
        //    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        //}

        //private static void SwaggerGen(SwaggerGenOptions options)
        //{
        //    //options.SchemaFilter<AddFluedValidationRulesToSwagger>();
        //    options.DescribeAllEnumsAsStrings();
        //    foreach (var file in Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.xml"))
        //    {
        //        options.IncludeXmlComments(file);
        //    }
        //}

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

            ConfigureApiVersioning(applicationBuilder, hostingEnvironment, apiVersionDescriptionProvider);
        }

        #region Private Methods

        private void AddApiVersioning(IServiceCollection services)
        {
            var swaggerOptions = new API.Swagger.SwaggerOptions();
            _configuration.GetSection(nameof(API.Swagger.SwaggerOptions)).Bind(swaggerOptions);

            var openApiInfo = new OpenApiInfo();
            _configuration.GetSection(nameof(OpenApiInfo)).Bind(openApiInfo);

            //services.Configure<OpenApiInfo>(Configuration.GetSection(nameof(OpenApiInfo)));

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
            IWebHostEnvironment hostingEnvironment,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            var swaggerOptions = new API.Swagger.SwaggerOptions();
            _configuration.GetSection(nameof(API.Swagger.SwaggerOptions)).Bind(swaggerOptions);

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