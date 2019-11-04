using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Morsley.UK.YearPlanner.Users.API.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly OpenApiInfo _openApiInfo;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IOptions<OpenApiInfo> options)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _openApiInfo = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Configure(SwaggerGenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));

            var info = new OpenApiInfo
            {
                Title = string.Format(_openApiInfo.Title, description.ApiVersion.ToString()),
                Version = description.ApiVersion.ToString(),
                Description = _openApiInfo.Description,
                Contact = new OpenApiContact
                {
                    Name = _openApiInfo.Contact.Name,
                    Email = _openApiInfo.Contact.Email,
                    Url = _openApiInfo.Contact.Url
                },
                TermsOfService = _openApiInfo.TermsOfService,
                License = new OpenApiLicense
                {
                    Name = _openApiInfo.License.Name,
                    Url = _openApiInfo.License.Url
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}