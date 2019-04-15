using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Data.Interfaces.Authentication;
using DatingApp.API.Data.Repositories.Authentication;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // this was added because the request from localhost:4200 (SPA) was not allowed to make the call to our API
            services.AddCors();
            // injecting the repositories and interfaces for use by project controllers
            services.AddScoped<IAuthRepository, AuthRepository>();
            // tell .NET Core what kind of middleware to use for authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // specify the options to validate against
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        // store the server error in the variable below
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error != null) 
                        {
                            // before writing the response, add the custom header to the response
                            context.Response.AddApplicationError(error.Error.Message);
                            // write the error message into the HTTP response
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            // this step is a very loose --for DEV only-- CORS solution to allow *any request to the application API
            app.UseCors(action => action.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            // the middleware to handle authentication is being injected above in the ConfigureServices, now tell the application to use the DI middleware
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
