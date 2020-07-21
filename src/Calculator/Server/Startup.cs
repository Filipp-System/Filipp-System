using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Calculator.BaseRepository;
using Calculator.DataAccess;
using Calculator.Model;
using Calculator.Repository;
using Calculator.Server.Data;
using Calculator.Server.Models;

namespace Calculator.Server
{
    public class Startup
    {
        private static readonly string DefaultConnection =
            nameof(DefaultConnection);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationFilippSystemDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("blazorfilippsystemdb")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationFilippSystemDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationFilippSystemDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddDbContextFactory<EmployeeContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString(EmployeeContext.BlazorFilippSystemDb))
                    .EnableSensitiveDataLogging());

            // add the repository
            services.AddScoped<IRepository<Employee, EmployeeContext>, EmployeeRepository>();
            services.AddScoped<IBasicRepository<Employee>>(sp =>
                sp.GetService<IRepository<Employee, EmployeeContext>>());
            services.AddScoped<IUnitOfWork<Employee>, UnitOfWork<EmployeeContext, Employee>>();

            // seeding the first time
            services.AddScoped<FilippSystemSeed>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
