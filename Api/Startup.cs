using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Errors;
using Api.Helpers;
using AutoMapper;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(s => s.Value.Errors.Count > 0)
                    .SelectMany(s => s.Value.Errors)
                    .Select(e => e.ErrorMessage).ToArray(); ;

                    var response = new BadRequestResponse(StatusCodes.Status400BadRequest, errors: errors);
                    return new BadRequestObjectResult(response);
                };
            });

            services.AddDbContext<StoreContext>(builder =>
            {
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultDatabase"));
            });

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "E-Commerce API" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";
                        var errors = context.Features.Get<IExceptionHandlerFeature>();

                        var response = new ApiResponse(StatusCodes.Status500InternalServerError, errors.Error.Message);
                        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                        var json = JsonSerializer.Serialize(response, options);
                        await context.Response.WriteAsync(json);
                    });
                });
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API V1");
            });

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
