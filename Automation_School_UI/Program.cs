using Automation_School_UI;
using Automation_School_UI.Providers;
using Automation_School_UI.Services;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

 Console.WriteLine(builder.HostEnvironment.Environment);

// switch (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
// {
//     case "Docker":
//         Console.WriteLine("Docker settings...");
       
//         break;
//     default:
//         Console.WriteLine("Default settings...");
       
//         break;
// }

//builder.Configuration.AddJsonFile("wwwroot/appsettings.json", optional: false, reloadOnChange: true);
IConfiguration conf = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // reloadOnChange Whether the configuration should be reloaded if the file changes.
            .AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true, reloadOnChange: true)
            .Build();

builder.Configuration.AddConfiguration(conf);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazorBootstrap();

builder.Services.AddAuthorizationCore();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IRollcallService, RollcallService>();



await builder.Build().RunAsync();
