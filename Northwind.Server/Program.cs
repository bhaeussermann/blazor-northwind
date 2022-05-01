using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
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

var apiUri = new Uri(builder.Configuration.GetValue<string>("NorthwindApiUri"));
var httpClient = new HttpClient { BaseAddress = apiUri };
app.MapMethods("/api/{**path}", new[] { HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete }, async context =>
{
    string apiPath = (string)context.GetRouteValue("path");
    var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), apiPath);
    if ((context.Request.Method == HttpMethods.Post) || (context.Request.Method == HttpMethods.Put))
    {
        string requestBody;
        using (var stringReader = new StreamReader(context.Request.Body))
        {
            requestBody = await stringReader.ReadToEndAsync();
        }
        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
    }

    try
    {
        var response = await httpClient.SendAsync(request);
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
    }
    catch (HttpRequestException exception)
    {
        context.Response.StatusCode = (int)(exception.StatusCode ?? HttpStatusCode.InternalServerError);
        await context.Response.WriteAsync(exception.Message);
    }
});

app.Run();
