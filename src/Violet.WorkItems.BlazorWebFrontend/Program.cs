using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Violet.WorkItems.BlazorWebFrontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<WorkItemService>();
builder.Services.AddScoped<WorkItemDescriptorService>();
builder.Services.AddScoped<ValueProviderService>();
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
