using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Budgeting.App.Api.Registrars
{
    public class IdentityRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            #region Add jwt settings
            var jwtSettings = new JwtSettings();

            builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
            var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));

            builder.Services.Configure<JwtSettings>(jwtSection);
            builder.Services.AddScoped<JwtSettings>();
            #endregion

            //add mongoIdentityConfiguration
            var mogoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = builder.Configuration["MongoDbOptions:ConnectionString"],
                    DatabaseName = builder.Configuration["MongoDbOptions:DatabaseName"]
                },
                IdentityOptionsAction = (options) =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 8;

                    options.User.RequireUniqueEmail = true;
                }
            };

            builder.Services.ConfigureMongoDbIdentityUserOnly<User, Guid>(mogoDbIdentityConfiguration)
                .AddUserManager<UserManager<User>>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudiences = jwtSettings.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Audience = jwtSettings.Audiences[0];
                options.ClaimsIssuer = jwtSettings.Issuer;
            });
        }
    }
}
