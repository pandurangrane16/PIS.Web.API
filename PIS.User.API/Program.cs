using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PIS.Framework;
using PIS.Framework.Authentication;
using PIS.User.API.Extensions;
using PIS.User.Core;
using PIS.User.Core.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var config = builder.Configuration;

// Setup JWT Signing Key
var signingKey = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes(config["Authentication:JwtBearer:SecurityKey"]));

// Configure strongly-typed JwtTokenConfiguration
var jwtConfig = new JwtTokenConfiguration
{
    Issuer = config["Authentication:JwtBearer:Issuer"],
    Audience = config["Authentication:JwtBearer:Audience"],
    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
    StartDate = DateTime.UtcNow,
    EndDate = DateTime.UtcNow.AddDays(60),
};

builder.Services.Configure<JwtTokenConfiguration>(options =>
{
    options.Audience = jwtConfig.Audience;
    options.EndDate = jwtConfig.EndDate;
    options.Issuer = jwtConfig.Issuer;
    options.StartDate = jwtConfig.StartDate;
    options.SigningCredentials = jwtConfig.SigningCredentials;
});

// Build connection string and register DbContext
var connectionStringSection = config.GetSection("ConnectionString");
builder.Services.Configure<ConnectionString>(connectionStringSection);
var connectionString = connectionStringSection.Get<ConnectionString>();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString.ToString()));
builder.Services.AddDataAccess<UserContext>();
builder.Services.AddScoped<IErrorLog, ErrorLogService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddInternalServices();


// Add controllers and JSON options
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = signingKey
    };
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PIS-User-WEB-API-Service", Version = "v1" });
});

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "PIS User API",
//        Version = "v1",
//        Description = "Swagger for PIS User API",
//    });
//    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseRouting();
app.UseAuthentication(); // Make sure this is added
app.UseAuthorization();

app.MapControllers();

app.Run();
