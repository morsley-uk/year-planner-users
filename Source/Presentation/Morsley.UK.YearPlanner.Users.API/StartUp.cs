using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Morsley.UK.YearPlanner.Users.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwaggerOptions = Swashbuckle.AspNetCore.Swagger.SwaggerOptions;

namespace Morsley.UK.YearPlanner.Users.API
{
    public class StartUp
    {
        public IConfiguration Configuration { get; }

        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Dependency Injection...
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AddApiVersioning(services);
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

            ConfigureApiVersioning(applicationBuilder, hostingEnvironment, apiVersionDescriptionProvider);
        }

        #region Private Methods

        private void AddApiVersioning(IServiceCollection services)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

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
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

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