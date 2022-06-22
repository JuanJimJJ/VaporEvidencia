using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VaporApp.Application;
using VaporApp.Application.Interfaces;
using VaporApp.Application.Services;
using VaporApp.Domain.Interfaces;
using VaporApp.Infrastructure.Filters;
using VaporApp.Infrastructure.Persistance;
using VaporApp.Infrastructure.Repositories;

namespace VaporApp.Api
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
            services
                .AddControllers(options =>
                {
                    //Setting global exceptions
                    options.Filters.Add<GlobalExceptionFilter>();
                }).AddFluentValidation();

            services.AddApplicationServices();

            //Linking chain of connection with DBContext
            services.AddDbContext<DBVaporContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Vapor")));

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductCategoriesRepository, ProductCategoriesRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();
            services.AddTransient<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();

            //Dependencies of services
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IProductCategoriesService, ProductCategoriesService>();
            services.AddTransient<IOrderDetailsService, OrderDetailsService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //Defining token parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44306",
                    ValidAudience = "https://localhost:44306",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("keykeykeykeykeykeykeykey"))
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ver1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Vapor API",
                    Description = "Available end-points!"
                });

                var nameFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, nameFile);

                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ver1/swagger.json", "ver1 Vapor API");

                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
