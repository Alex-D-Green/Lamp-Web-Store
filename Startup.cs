using LampWebStore.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LampWebStore
{
    public class Startup
    {
        /// <summary>The object to work with configuration.</summary>
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration) //DI...
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding MVC infrastructure to DI
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            //Getting the default DB connection string from configuration and adding DB context to DI
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LampsContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error"); //To hide sensitive info in production

            app.UseStaticFiles(); //To gain access to the static files in wwwroot
            app.UseStatusCodePages(); //To see HTTP status codes (very basic way to handle HTTP errors)

            //Adding MVC into the pipeline and defining routes
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
