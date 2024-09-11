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


builder.Services.AddBlazoredLocalStorageAsSingleton();


builder.Services.AddBlazorBootstrap();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IContactsService<ContactMeDTO>, ContactsService>();
// builder.Services.AddScoped<IBlo<ContactMeDTO>, ContactsService>();


await builder.Build().RunAsync();
