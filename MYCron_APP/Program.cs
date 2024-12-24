using Microsoft.EntityFrameworkCore;
using MYCentralModels;
using MYCentralModels.SQLite;
using System.Text.Json;
using MYCron_APP.DbContextLite;
using MYCron_APP.Pages.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<VisitorTrackingMiddleware>();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
});

builder.Services.AddSingleton(provider => GetInternalURLsAsync(provider).GetAwaiter().GetResult());
builder.Services.AddSingleton(provider => GetVisitUsInfosAsync(provider).GetAwaiter().GetResult());
builder.Services.AddSingleton(provider => GetOutOfSessionPagesAsync(provider).GetAwaiter().GetResult());

builder.Services.AddDbContext<SqLiteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqLiteDbConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel:Limits"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<VisitorTrackingMiddleware>();
app.UseMiddleware<SessionHandlerMiddleware>();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

async Task<List<VisitUsInformationModel>> GetVisitUsInfosAsync(IServiceProvider provider)
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    var internalUrls = provider.GetRequiredService<List<InternalUrl>>();
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    try
    {
        var response = await httpClient.GetAsync(CommonAssets.ApiUrl(internalUrls, 0, "GetVisitUsInfoList"));
        response.EnsureSuccessStatusCode();
        var responseJSON = await response.Content.ReadAsStringAsync();
        var visitUsInfos = JsonSerializer.Deserialize<List<VisitUsInformationModel>>(responseJSON, CommonAssets.JsonSerializerOptions);
        return visitUsInfos;
    }
    catch (HttpRequestException)
    {
        // Handle the exception
        return new List<VisitUsInformationModel>(); // Return an empty list or handle the error appropriately
    }
}

async Task<List<OutOfSessionPage>> GetOutOfSessionPagesAsync(IServiceProvider provider)
{
    using var scope = provider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
    return await dbContext.OutOfSessionPages.ToListAsync();
}
async Task<List<InternalUrl>> GetInternalURLsAsync(IServiceProvider provider)
{
    using var scope = provider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
    return await dbContext.InternalUrls.ToListAsync();
}