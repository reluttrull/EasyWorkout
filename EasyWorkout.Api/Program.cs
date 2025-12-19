using EasyWorkout.Application.Data;
using Microsoft.EntityFrameworkCore;

string root = Directory.GetCurrentDirectory();
string solutionDotEnvPath = Path.Combine(root, "../.env");
if (File.Exists(solutionDotEnvPath))
{
    DotNetEnv.Env.Load(solutionDotEnvPath);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = Environment.GetEnvironmentVariable("WORKOUTSDB_CONNECTIONSTRING");

builder.Services.AddDbContext<WorkoutsContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var workoutsDb = scope.ServiceProvider.GetRequiredService<WorkoutsContext>();
    SeedData.Initialize(workoutsDb);
}

app.MapFallbackToFile("/index.html");

app.Run();
