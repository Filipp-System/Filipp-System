using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Calculator.BaseRepository;
using Calculator.Client.Data;
using Calculator.Controls.Grid;
using Calculator.Model;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Client
{
    public class Program
    {
        public const string BaseClient = "Calculator.ServerAPI";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>(nameof(App).ToLowerInvariant());

            builder.Services.AddHttpClient(BaseClient, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => 
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(BaseClient));

            builder.Services.AddApiAuthorization();

            // client implementation
            builder.Services.AddScoped<IBasicRepository<Employee>, WasmRepository>();
            builder.Services.AddScoped<IUnitOfWork<Employee>, WasmUnitOfWork>();
            
            // Add DevExpress Blazor Controls
            builder.Services.AddDevExpressBlazor();

            // references to control filters and sorts
            builder.Services.AddScoped<IPageHelper, PageHelper>();
            builder.Services.AddScoped<IEmployeeFilters, EmployeeFilters>();
            builder.Services.AddScoped<GridQueryAdapter>();
            // builder.Services.AddScoped<EditService>(); TODO: implement the EditService

            builder.Services.AddScoped(sp =>
                new ClaimsPrincipal(new ClaimsIdentity()));

            await builder.Build().RunAsync();
        }
    }
}
