using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Budget.Web.App_Start
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseFileServer();
            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin());

            app.UseQueryContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // how can you ever know if this is safe to be a singleton?
            services.AddSingleton<Budget.Statements.IStatementProvider, Budget.Westpac.StatementProvider>();
            services.AddSingleton<Budget.ITagsProvider, Westpac.TagsProvider>();
            services.AddSingleton<Budget.Westpac.Configuration>(provider => Configuration.GetInstance<Budget.Westpac.Configuration>());

            services.AddTransient<Budget.Service>();
        }
    }
}
