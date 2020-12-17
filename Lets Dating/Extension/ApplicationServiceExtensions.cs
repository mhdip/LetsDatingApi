using Lets_Dating.Data;
using Lets_Dating.interfaces;
using Lets_Dating.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets_Dating.Extension
{
    //Extension method 
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // This refer life time of services. How long this service will use. AddScoped() -> scoped the life time of http request
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
