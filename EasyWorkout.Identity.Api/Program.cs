using DotNetEnv;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


string root = Directory.GetCurrentDirectory();
//string solutionEnvironmentPath = Path.Combine(root, $"../.env.production"); // for testing env
string solutionEnvironmentPath = Path.Combine(root, $"../.env.{builder.Environment.EnvironmentName}");
string solutionDefaultPath = Path.Combine(root, "../.env");

if (builder.Environment.IsDevelopment())
{
    if (File.Exists(solutionEnvironmentPath))
    {
        Env.Load(solutionEnvironmentPath);
    }
    else
    {
        // fallback to the default .env if the specific one isn't found
        Env.Load(solutionDefaultPath);
    }
}

var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? builder.Configuration["AWS_ACCESS_KEY"];
var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY") ?? builder.Configuration["AWS_SECRET_KEY"];
var bucketPath = Environment.GetEnvironmentVariable("AWS_BUCKET_PATH") ?? builder.Configuration["AWS_BUCKET_PATH"];

Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Debug()
    .WriteTo.AmazonS3(
        path: "log.txt",
        bucketName: "easyworkout-backups-logs",
        bucketPath: bucketPath,
        endpoint: Amazon.RegionEndpoint.USEast2,
        awsAccessKeyId: awsAccessKey,
        awsSecretAccessKey: awsSecretKey,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: Serilog.Sinks.AmazonS3.RollingInterval.Day,
        batchSizeLimit: 20,
        batchingPeriod: TimeSpan.FromSeconds(30),
        eagerlyEmitFirstEvent: true,
        queueSizeLimit: 5000,
        disablePayloadSigning: false,
        failureCallback: ex => Console.WriteLine($"S3 sink failed: {ex.Message}")
    )
    .CreateLogger();

builder.Host.UseSerilog();

//builder.WebHost.UseKestrel(options =>
//{
//    options.AddServerHeader = false;
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins("https://reluttrull.github.io", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var config = builder.Configuration;

// Add services to the container.
var usersConnectionString = Environment.GetEnvironmentVariable("USERSDB_CONNECTIONSTRING") ?? builder.Configuration["USERSDB_CONNECTIONSTRING"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(usersConnectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration["JWT_ISSUER"],
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? builder.Configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_SECRET") ?? builder.Configuration["TOKEN_SECRET"]!)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowClient");

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
