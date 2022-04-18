﻿var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

var httpClient = new HttpClient { BaseAddress = new Uri("https://northwind-express-api.herokuapp.com/") };
app.MapMethods("/api/{**path}", new[] { HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete }, async context =>
{
    string apiPath = (string)context.GetRouteValue("path");
    var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method), apiPath));
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
});

app.Run();
