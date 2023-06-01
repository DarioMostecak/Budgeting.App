using Budgeting.App.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Budgeting.App.Api.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            #region Add Mongo options
            var mongoDbOptions = new MongoDbOptions();

            builder.Configuration.Bind(nameof(MongoDbOptions), mongoDbOptions);
            var mongoDbOptionsSection = builder.Configuration.GetSection(nameof(MongoDbOptions));

            builder.Services.Configure<MongoDbOptions>(mongoDbOptionsSection);
            builder.Services.AddScoped<MongoDbOptions>();
            #endregion

            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
        }
    }
}
