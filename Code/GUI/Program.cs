using ClassLibrary;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GUI;
using GUI.Coordinators;
using GUI.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri($"http://localhost:{GlobalConstants.BackendPort}")
});
builder.Services.AddSingleton<StoryEditCoordinator>();
builder.Services.AddScoped<StoryService>();
builder.Services.AddMudServices();

var app = builder.Build();

await app.RunAsync();