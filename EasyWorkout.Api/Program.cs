using EasyWorkout.Application.Data;
using EasyWorkout.Application.Services;
using EasyWorkout.Identity.Api;
using EasyWorkout.Identity.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Debug()
    .CreateLogger();

try
{
    string root = Directory.GetCurrentDirectory();
    string solutionDotEnvPath = Path.Combine(root, "../.env");
    if (File.Exists(solutionDotEnvPath))
    {
        DotNetEnv.Env.Load(solutionDotEnvPath);
    }

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.WebHost.UseKestrel(options =>
    {
        options.AddServerHeader = false;
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowClient", policy =>
            policy.WithOrigins("https://reluttrull.github.io", "https://localhost:55169")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });

    var config = builder.Configuration;

    var workoutsConnectionString = Environment.GetEnvironmentVariable("WORKOUTSDB_CONNECTIONSTRING");
    var usersConnectionString = Environment.GetEnvironmentVariable("USERSDB_CONNECTIONSTRING");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(usersConnectionString));

    builder.Services.AddDbContext<WorkoutsContext>(options =>
        options.UseNpgsql(workoutsConnectionString));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_SECRET")!)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };
        });

    builder.Services.AddAuthorization(x =>
    {
        x.AddPolicy(AuthConstants.AdminUserPolicyName,
            p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));
        x.AddPolicy(AuthConstants.PaidMemberUserPolicyName,
            p => p.RequireAssertion(c => c.User.HasClaim(m => m is { Type: AuthConstants.PaidMemberUserClaimName, Value: "true" })
                || c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" })));
        x.AddPolicy(AuthConstants.FreeMemberUserPolicyName,
            p => p.RequireAssertion(c => c.User.HasClaim(m => m is { Type: AuthConstants.FreeMemberUserClaimName, Value: "true" })
                || c.User.HasClaim(m => m is { Type: AuthConstants.PaidMemberUserClaimName, Value: "true" })
                || c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" })));
    });

    builder.Services.AddControllers();

    builder.Services.AddOpenApi();
    builder.Services.AddOpenApiDocument();

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

    builder.Services.AddScoped<IWorkoutService, WorkoutService>();
    builder.Services.AddScoped<IExerciseService, ExerciseService>();
    builder.Services.AddScoped<ICompletedWorkoutService, CompletedWorkoutService>();
    builder.Services.AddScoped<IUserService, UserService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseDefaultFiles();
    app.MapStaticAssets();

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.MapOpenApi();
        app.UseSwaggerUi();
    }

    app.UseCors("AllowClient");

    app.UseHttpsRedirection();

    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    //var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    //using (var scope = scopeFactory.CreateScope())
    //{
    //    var usersDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //    SeedUserData.Initialize(usersDb);
    //    var workoutsDb = scope.ServiceProvider.GetRequiredService<WorkoutsContext>();
    //    SeedData.Initialize(workoutsDb, [.. usersDb.Users]);
    //}

    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}