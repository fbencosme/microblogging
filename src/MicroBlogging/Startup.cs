using MicroBlogging.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace MicroBlogging
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<MicroBloggerContext>();

            services.AddMvc()
                .AddJsonOptions(
                    options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
            services.AddCaching();
            services.AddSession();
            services.Configure<MvcOptions>(options =>
            {
                // options.Filters.Add(new ValidateAttributes());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();
            app.UseSession();

            app.UseCookieAuthentication();
            app.UseStaticFiles();
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            // app.UseIdentity();
        }


        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}