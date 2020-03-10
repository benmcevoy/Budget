using System;
using System.Threading.Tasks;
using Budget.Facets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Budget.Web
{
    public class QueryContext 
    {
        private readonly RequestDelegate _next;
        public QueryContext(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var instance = (Query)context.Items[GetType()] ?? new Query
            {
                DateRange = Enum.TryParse<Dates.Range>(context.Request.Query["dateRange"], out var range)
                    ? range
                    : Dates.Range.LastMonth,
                DateResolution =
                    Enum.TryParse<Dates.Resolution>(context.Request.Query["dateResolution"], out var resolution)
                        ? resolution
                        : Dates.Resolution.Day
            };

            context.Items[GetType()] = instance;

            Query = instance;

            await _next(context);
        }

        public static Query Query;
    }

    public class Query
    {
        public Dates.Range DateRange { get; set; } = Dates.Range.LastMonth;
        public Dates.Resolution DateResolution { get; set; } = Dates.Resolution.Day;
    }

    public static class QueryContextExtensions
    {
        public static IApplicationBuilder UseQueryContext(this IApplicationBuilder app) => app.UseMiddleware<QueryContext>();
    }
}
