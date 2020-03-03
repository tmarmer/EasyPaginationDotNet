using System;
using System.Web;
using EasyPagination.AspNetCore.Enums;
using EasyPagination.AspNetCore.PageCalculation;
using EasyPagination.AspNetCore.Params;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EasyPagination.AspNetCore
{
    public static class Extensions
    {
        public static IServiceCollection UsePagination<T>(this IServiceCollection serviceCollection,
            PaginationType paginationType) where T : IPaginationParams
        {
            return paginationType switch
            {
                PaginationType.LimitItems => serviceCollection.UsePagination(options =>
                {
                    options.RegisterPageCalculator<T>(new LimitOffsetCalculator());
                }),
                PaginationType.Pages => serviceCollection.UsePagination(options =>
                {
                    options.RegisterPageCalculator<T>(new PagesCalculator());
                }),
                _ => throw new ArgumentException("Invalid pagination type", nameof(paginationType))
            };
        }

        public static void AddPaginationOptions(this MvcOptions options)
            => options.Filters.Add<PaginationActionFilter>();

        public static IServiceCollection UsePagination(this IServiceCollection serviceCollection,
            Action<PageCalculationOptions> options)
        {
            var pageCalc = new PageCalculationOptions();
            options(pageCalc);
            return serviceCollection.AddSingleton<IPageCalculationService>(pageCalc.GenerateCalculationService());
        }

        public static string GetLinkHeaderValue(this LinkRelationship linkRelationship)
        {
            return linkRelationship switch
            {
                LinkRelationship.First => "first",
                LinkRelationship.Previous => "prev",
                LinkRelationship.Next => "next",
                LinkRelationship.Last => "last",
                _ => string.Empty
            };
        }

        public static Uri GenerateLinkHeaderUri(this IPaginationParams paginationParams, Uri baseUri, int newOffset)
        {
            var queryString = HttpUtility.ParseQueryString(baseUri.Query);
            queryString[paginationParams.GetPageSizeName()] = paginationParams.GetPageSize().ToString();
            queryString[paginationParams.GetOffsetName()] = newOffset.ToString();

            if (!string.IsNullOrEmpty(baseUri.Query))
                return new Uri(baseUri.ToString().Replace(baseUri.Query, $"?{queryString}"));

            return new Uri($"{baseUri}?{queryString}");
        }

        public static void EnablePagination(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.OperationFilter<PaginationOperationFilter>();
        }

        public static void RegisterDefaultCalculators(this PageCalculationOptions options)
            => options.RegisterDefaultCalculators<PagesParams, LimitOffsetParams>(new PagesCalculator(), new LimitOffsetCalculator());
        
        public static void RegisterDefaultCalculators<TPageType, TLimitType>(this PageCalculationOptions options)
            where TPageType : IPaginationParams
            where TLimitType : IPaginationParams
            => options.RegisterDefaultCalculators<TPageType, TLimitType>(new PagesCalculator(), new LimitOffsetCalculator());

        private static void RegisterDefaultCalculators<TPageType, TLimitType>(this PageCalculationOptions options,
            IPageCalculator pagesCalculator, IPageCalculator limitOffsetCalculator)
            where TPageType : IPaginationParams
            where TLimitType : IPaginationParams
        {
            options.RegisterPageCalculator<TPageType>(pagesCalculator);
            options.RegisterPageCalculator<TLimitType>(limitOffsetCalculator);
        }
    }
}