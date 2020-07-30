using System;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Calculator.BaseRepository;
using Calculator.DataAccess;
using Calculator.Models.DatabaseModels;
using Calculator.Models.Identity;
using Calculator.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDbGenericRepository;

namespace Calculator.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnectionString = Configuration.GetConnectionString("MongoServer");
            var mongoDatabase = Configuration.GetConnectionString("MongoDatabase");

            var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = mongoConnectionString,
                    DatabaseName = mongoDatabase
                },
                IdentityOptionsAction = options =>
                {
                    // password requirements
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;

                    // lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    // ApplicationUser settings
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

                    // SignIn requirements
                    options.SignIn.RequireConfirmedAccount = true;
                }
            };

            services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfiguration);
            //services.AddAuthentication()
            //    .AddIdentityServerJwt();


            //services.AddDbContext<ApplicationFilippSystemDbContext>(options =>
            //    options.UseSqlite(
            //        Configuration.GetConnectionString("DefaultConnection")));


            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationFilippSystemDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, ApplicationFilippSystemDbContext>();



            //services.AddDbContextFactory<FilippSystemContext>(opt =>
            //    opt.UseSqlite(Configuration.GetConnectionString(FilippSystemContext.BlazorFilippSystemDb))
            //        .EnableSensitiveDataLogging());

            //add the repository
            //services.AddScoped<IDbContextFactory<FilippSystemContext>, DbContextFactory<FilippSystemContext>>();
            //services.AddScoped<IRepository<Calculation, FilippSystemContext>, CalculationRepository>();
            //services.AddScoped<IBasicRepository<Calculation>>(sp =>
            //    sp.GetService<IRepository<Calculation, FilippSystemContext>>());
            //services.AddScoped<IUnitOfWork<Calculation>, UnitOfWork<FilippSystemContext, Calculation>>();

            // seeding the first time
            //services.AddScoped<EmployeeSeed>();

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

            //app.UseIdentityServer();
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
