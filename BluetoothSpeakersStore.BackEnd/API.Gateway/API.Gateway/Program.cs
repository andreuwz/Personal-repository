using API.Gateway.Middleware;
using API.Gateway.Services;
using API.Gateway.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.
     AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
            //-> If the Htpp request from Angular has option withCredentials : true
            // , this line should be uncommented
        });
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IDataSecurityService,DataSecurityService>();
builder.Services.AddTransient<ITokenService, TokenService>();
IConfiguration configuration = builder.Configuration.AddJsonFile($"ocelot.json", true, true).Build();
builder.Services.AddOcelot(configuration);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header// and configures them to validate the token with the options.AddJwtBearer(options =>
.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:7254";
    options.Audience = "https://localhost:7254/resources";
});

builder.Services.AddAuthorization();
var app = builder.Build();

app.UseCors(builder =>
     builder
  .WithOrigins("http://localhost:4200")
  .AllowAnyHeader()
  .AllowCredentials()
  .AllowAnyMethod());

app.UseMiddleware<AccessTokenDecryptor>();
app.UseMiddleware<AccessTokenRefresher>();
app.UseOcelot().Wait();
app.Run();
