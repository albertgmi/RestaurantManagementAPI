using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI;
using RestaurantManagementAPI.Entities;
using System.Text.Json.Serialization;
using NLog.Web;
using RestaurantManagementAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantManagementAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using RestaurantManagementAPI.Services.UserServiceFolder;
using RestaurantManagementAPI.Seeders;
using RestaurantManagementAPI.Services.RestaurantServiceFolder;
using RestaurantManagementAPI.Services.DishServiceFolder;


var builder = WebApplication.CreateBuilder(args);

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";

}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
    options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
    options.AddPolicy("Atleast2restaurants", builder => builder.AddRequirements(new AtleastTwoRestaurantsRequirement(2)));
});

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AtleastTwoRestaurantsHandler>();
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Host.UseNLog();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<TimeRequestMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", policyBuilder =>
    {
        policyBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(builder.Configuration["AllowedOrigins"]);
    });
});

// Entity Framework stuff
builder.Services.AddDbContext<RestaurantDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantConnectionString")));
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddTransient<IRestaurantSeeder, RestaurantSeeder>();


var app = builder.Build();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontendClient");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<TimeRequestMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI"));
app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
var dataSeeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
dataSeeder.Seed(dbContext);

app.Run();

public partial class Program { }