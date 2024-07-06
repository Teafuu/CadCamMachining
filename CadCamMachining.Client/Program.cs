using CadCamMachining.Client;
using CadCamMachining.Client.Services;
using CadCamMachining.Client.Services.Contracts;
using CadCamMachining.Client.Services.Implementations;
using CadCamMachining.Client.States;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthorizeApi, AuthorizeApi>();
builder.Services.AddSingleton<ItemFacade>();
builder.Services.AddSingleton<ItemSelectionManager>();
builder.Services.AddSingleton<LayoutFacade>();

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();