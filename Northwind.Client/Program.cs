using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Northwind.Client;
using Northwind.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<Radzen.NotificationService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpClient<EmployeeDataService>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/employees"));

await builder.Build().RunAsync();
