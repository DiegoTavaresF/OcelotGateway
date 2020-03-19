using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Text;

namespace ExampleGateway.Gateway
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "TestKey";

            // get from appsettings.json
            var mySecuritySettings = new
            {
                Secret = "P8m<*z}*Y?duu5Bs8y;gup6]&6~%cn~`u7gNaf526$~z_zbDPtkRF*}cTJ{3",
                ExpirationMinutes = 30,
                Issuer = "SampleIssuer",
                ValideIn = "https://localhost"
            };
            var key = Encoding.ASCII.GetBytes(mySecuritySettings.Secret);

            var identityUrl = _configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, x =>
                {
                    //x.Authority = identityUrl;
                    //x.RequireHttpsMetadata = false;
                    //x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    //{
                    //    ValidAudiences = new[] { "select", "houston", "visit" }
                    //};

                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = mySecuritySettings.Issuer,
                        ValidAudience = mySecuritySettings.ValideIn,
                    };

                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = tokenValidationParameters;

                    x.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            int i = 0;
                        },
                        OnTokenValidated = async ctx =>
                        {
                            int i = 0;
                        },

                        OnMessageReceived = async ctx =>
                        {
                            int i = 0;
                        }
                    };
                });

            services.AddOcelot(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseOcelot().Wait();
        }
    }
}