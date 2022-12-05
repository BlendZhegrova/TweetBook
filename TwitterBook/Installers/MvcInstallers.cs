using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TwitterBook.Controllers.V1;
using TwitterBook.Options;
using TwitterBook.Services;

namespace TwitterBook.Installers
{
    public class MvcInstallers : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
        var  jwtSettings = new JwtSettings();
        var token = jwtSettings.Secret;
        configuration.Bind(nameof(jwtSettings), jwtSettings);
        services.AddSession();
        services.AddSingleton(jwtSettings);
        services.AddControllersWithViews();
        services.AddTransient<IIdentityService, IdentityService>();

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = tokenValidationParameters;
        });
        services.AddAuthorization();
    services.AddSwaggerGen(swaggerGenOptions =>
    {
        swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ASP.NET tutorial", Version = "v1"
        });
        var security = new Dictionary<string, IEnumerable<string>>
        {
            { "Bearer", new string[0] }
        };
        swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the bearer scheme",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        // swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement()
        // {
        //     {
        //         new OpenApiSecurityScheme
        //         {
        //             Reference = new OpenApiReference
        //             {
        //                 Type = ReferenceType.SecurityScheme,
        //                 Id = "Bearer"
        //             },
        //             Scheme = "oauth2",
        //             Name = "bearer",
        //             In = ParameterLocation.Header,
        //         },
        //         new List<string>()
        //     }
        // });
        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

}
}