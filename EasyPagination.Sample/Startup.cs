using System;
using EasyPagination.AspNetCore;
using EasyPagination.AspNetCore.Enums;
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
        public void ConfigureServices(IServiceCollection services)
        {
            //Setup controllers
            services.AddControllers();
            
            //Setup swagger if you want
            services.AddSwaggerGen(c =>
            {
                //Add pagination features to swagger to ensure correct status codes are displayed
                c.EnablePagination();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            
            /*
             * To enable simple pagination using just the default built-in types, replace the below code with one of these:
             * services.UsePagination<LimitOffsetParams>(PaginationType.LimitItems);
             * --- OR ---
             * services.UsePagination<PagesParams>(PaginationType.Pages);
             */

            //Enable pagination with custom options
            services.UsePagination(pageCalculationOptions =>
            {
                //Register calculators for built-in param types: LimitOffsetParams, PagesParams
                pageCalculationOptions.RegisterDefaultCalculators();
                
                //Register custom page calculators to create new links in the Link header
                pageCalculationOptions.RegisterPageCalculator<MyPaginationParams>(opts =>
                {
                    //Use the same link calculation for First, Last, Prev, and Next links as the default for LimitOffsetParams
                    opts.UseDefaultCalculation(PaginationType.LimitItems);
                    //Have an extra link with relationship "Test"
                    opts.SetPageCalculation("Test", info => new Uri($"{info.BaseUri}&hello=world"));
                });
            });
        }

        // No pagination options are setup in the configure step, so set this up however you want.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

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