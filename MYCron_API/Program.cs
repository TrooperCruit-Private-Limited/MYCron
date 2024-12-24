using MYCron_API;
using MYCentralModels;
using Microsoft.EntityFrameworkCore;
using MYCron_API.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
List<KeyValueModel> connectionStrings = [];
foreach (IConfigurationSection subSection in configuration.GetSection("ConnectionStrings").GetChildren())
{
    connectionStrings.Add(new() { Key = subSection.Key, Value = subSection.Value ?? "" });
}
builder.Services.AddDbContext<MYCronDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("TrooperCruitPostgreSQL")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.ConfigureServices((hostContext, services) =>
{
    services.AddSingleton(connectionStrings);
});
var app = builder.Build();

CommonEndPoints commonEndPoints = new();
ProjectManagerEndPoints projectManagerEndPoints = new();
commonEndPoints.WebSiteEndPoint(app);
projectManagerEndPoints.WebSiteEndPoint(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
