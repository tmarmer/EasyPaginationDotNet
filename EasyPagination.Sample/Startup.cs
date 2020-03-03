using System;
using System.IO;
using EasyPagination.AspNetCore;
using EasyPagination.AspNetCore.Enums;
using EasyPagination.AspNetCore.PageCalculation;
using EasyPagination.AspNetCore.Params;
using EasyPagination.Sample.Params;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EasyPagination.Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(x => x.AddPaginationOptions());
            services.AddSwaggerGen(c =>
            {
                c.EnablePagination();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.UsePagination(pageCalculationOptions =>
            {
                pageCalculationOptions.RegisterDefaultCalculators();
                pageCalculationOptions.RegisterPageCalculator<MyPaginationParams>(opts =>
                {
                    opts.UseDefaultCalculation(PaginationType.LimitItems);
                    opts.SetPageCalculation("woah", info => new PageData
                    {
                        PageLink = new Uri($"{info.BaseUri}&henlo=boba")
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapControllers();
            });
        }
    }
}