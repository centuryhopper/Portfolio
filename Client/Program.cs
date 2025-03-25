
global using HandyBlazorComponents.Components.Public;
global using HandyBlazorComponents.Components.Public.Modals;
global using Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Providers;
using Blazored.LocalStorage;
using Client.Interfaces;
using Client.Services;
using Client.Handlers;
using Client.Utils;
using Shared.Models;
using Blazored.SessionStorage;
using Fluxor;
using HandyBlazorComponents.Extensions;

/*
clear nuget cache if not getting the latest version:
- dotnet nuget locals all --clear && dotnet remove package HandyBlazorComponents ; dotnet add package HandyBlazorComponents
*/

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services
.AddHttpClient(
    Constants.HTTP_CLIENT,
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    }
)
// need this line for being able to use httpcontext.user.claims in the server controllers
.AddHttpMessageHandler<AuthorizationMessageHandler>();


builder.Services.AddScoped(
    sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(Constants.HTTP_CLIENT)
);

builder.Services.AddHandyBlazorServices();


builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazoredSessionStorage();


builder.Services.AddBlazorBootstrap();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IContactsService<ContactMeDTO>, ContactsService>();

builder.Services.AddFluxor(options =>
    options.ScanAssemblies(typeof(Program).Assembly));


await builder.Build().RunAsync();
