using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webApiCrudApplication.Common_Util;
using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.RepositoryLayer;
using webApiCrudApplication.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<SqlQueries>();



builder.Services.AddScoped<IInformationSL, CrudApplicationSL>();
builder.Services.AddScoped<IUserSL, CrudApplicationSL>();

builder.Services.AddScoped<IInformationRL, CrudApplicationRL>();
builder.Services.AddScoped<IUserRL, CrudApplicationRL>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // Ensure IConfiguration is registered
// Register Logger as Singleton and inject connection string
/*builder.Services.AddSingleton<Logger>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MySqlDBString");
    return new Logger(connectionString);  // Inject connection string from configuration
});*/
builder.Services.AddSingleton<webApiCrudApplication.Logs.ILogger>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MySqlDBString");
    return new webApiCrudApplication.Logs.Logger(connectionString);
});




builder.Services.Configure<ApiBehaviorOptions>(options =>
{
        // Resolve the logger from the service provider
    var serviceProvider = builder.Services.BuildServiceProvider();
    var logger = serviceProvider.GetRequiredService<webApiCrudApplication.Logs.ILogger>();
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage).ToList();
        // Log the validation errors
        logger.Log($"Validation errors occurred: {string.Join(", ", errors)}", null);
        var result = new
        {
            IsSuccess = false,
            Message = "Validation errors occurred.",
            Errors = errors

        };

        return new BadRequestObjectResult(result);
    };
});
// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,  // Ensures the token is not expired
        ClockSkew = TimeSpan.Zero  // Optional: Set to zero to reduce delay tolerance
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
