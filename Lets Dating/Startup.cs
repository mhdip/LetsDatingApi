using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Lets_Dating.Data;
using Lets_Dating.interfaces;
using Lets_Dating.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lets_Dating.Extension;

namespace Lets_Date
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // we use extenstion method for neat and clean configure services method, where we put sql server datacontext and life tiem scope of token services
            services.AddApplicationServices(_config);

            // for controller services
            services.AddControllers();

            // for cross origin 
            services.AddCors();

            // we use extenstion method for neat and clean configure services, where we put Authentication bearer and security 
            services.AddAuthenticationServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // if anything goes wrong show developer exception page
            }

            app.UseHttpsRedirection(); // for redirecting to http end point

            app.UseRouting(); // for allowing browser to show data through controller
             
            app.UseCors(poilicy=> poilicy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); // For allowing CORS Policiy

            app.UseAuthentication();

            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // look inside our controller and check wheather endpoints are available or not
            });
        }
    }
}
