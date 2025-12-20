using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.EntityFrameworkCore;

string root = Directory.GetCurrentDirectory();
string solutionDotEnvPath = Path.Combine(root, "../.env");
if (File.Exists(solutionDotEnvPath))
{
    DotNetEnv.Env.Load(solutionDotEnvPath);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var usersConnectionString = Environment.GetEnvironmentVariable("USERSDB_CONNECTIONSTRING");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(usersConnectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
