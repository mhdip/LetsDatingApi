using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lets_Dating.Extension
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration config)
        {
            // We need add authentication middle so before adding middle we need to configure authentication through JWT Bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // when our server sign the token we need to tell that it is validate
                        ValidateIssuerSigningKey = true,

                        // our symmetric key from devleopment.josn file "TokenKY"
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false, // Api server
                        ValidateAudience = false, // Angular Application
                    };
                });

            return services;
        }
    }
}
