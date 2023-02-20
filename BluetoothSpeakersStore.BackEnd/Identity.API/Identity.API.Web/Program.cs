using Identity.API.Application.Users.Commands.AssignRoleUser;
using Identity.API.Application.Users.Commands.CreateUser;
using Identity.API.Application.Users.Commands.DeleteUser;
using Identity.API.Application.Users.Commands.LoginUser;
using Identity.API.Application.Users.Commands.RegisterUser;
using Identity.API.Application.Users.Commands.UnassignRoleUser;
using Identity.API.Application.Users.Commands.UpdateUser;
using Identity.API.Application.Users.Commands.UpdateUserAdmin;
using Identity.API.Application.Users.Queries.GetAllUsers;
using Identity.API.Application.Users.Queries.GetPrincipalUser;
using Identity.API.Application.Users.Queries.GetUser;
using Identity.API.Application.Users.Validations;
using Identity.API.Domain;
using Identity.API.Persistence;
using Identity.API.Persistence.Repository;
using Identity.API.Persistence.RepositoryContract;
using Identity.API.Web.AsyncMessageBusServices;
using Identity.API.Web.AsyncMessageBusServices.EventProcessing;
using Identity.API.Web.AsyncMessageBusServices.PublishedMessages;
using Identity.API.Web.AsyncMessageBusServices.PublishMessages;
using Identity.API.Web.AsyncMessageBusServices.Rpc;
using Identity.API.Web.AuthorizationPolicies.Handler;
using Identity.API.Web.AuthorizationPolicies.Requirement;
using Identity.API.Web.IdentityServer;
using Identity.API.Web.IdentityServer.Contracts;
using Identity.API.Web.Middleware;
using Identity.API.Web.TransientFaultsPolicies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkforceManagementAPI.WEB.IdentityAuth;

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
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDefaultIdentity<User>(options =>
options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DatabaseContext>()
.AddSignInManager<SignInRepository>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//EF Identity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})

       //Injecting the services and DB in the DI containter
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<DatabaseContext>()
       .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
 // Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header// and configures them to validate the token with the options.AddJwtBearer(options =>
.AddJwtBearer("Bearer",options =>
{   
    options.Authority = "https://localhost:7254";
    options.Audience = "https://localhost:7254/resources";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy =>
                    policy.RequireRole("Administrator"));

    options.AddPolicy("MasterAdmin", policy =>
                    policy.RequireRole("MasterAdmin"));

    options.AddPolicy("RegularUser", policy =>
                    policy.RequireRole("RegularUser"));

    options.AddPolicy("AdminSelfActionOnly", policy =>
                policy.Requirements.Add(new ProhibitAdminActionRequirement()));
});

builder.Services.AddIdentityServer((options) =>
{
    options.EmitStaticAudienceClaim = true;
})

//This is for dev only scenarios when you don’t have a certificate to use..AddInMemoryApiScopes(IdentityConfig.ApiScopes)
.AddInMemoryClients(IdentityConfig.Clients)
.AddDeveloperSigningCredential()
.AddProfileService<ProfileService>()
.AddResourceOwnerValidator<PasswordValidator>()
.AddInMemoryApiScopes(IdentityConfig.ApiScopes);

builder.Services.AddHostedService<RpcServer>();
builder.Services.AddHostedService<MessageBusSubscriber>(); //RabbitMQ Async Bus Factory
builder.Services.AddSingleton<ConnectionState>();
builder.Services.AddTransient<IPublishNewMessage, PublishNewMessage>();
builder.Services.AddTransient<IUpdateUserBalance, UpdateUserBalance>();
builder.Services.AddTransient<ISendUserBalance, SendUserBalance>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton<TransientFaultPolicy>();
builder.Services.AddTransient<IDataSecurity, DataSecurity>();

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<ISignInRepository, SignInRepository>();
builder.Services.AddTransient<IUserValidations, UserValidations>();
builder.Services.AddTransient<IGetPrincipalUser, GetPrincipalUser>();
builder.Services.AddTransient<IGetAllUsers, GetAllUsers>();
builder.Services.AddTransient<IGetUser, GetUser>();
builder.Services.AddTransient<ICreateUser, CreateUser>();
builder.Services.AddTransient<IUpdateUserAdmin, UpdateUserAdmin>();
builder.Services.AddTransient<IDeleteUser, DeleteUser>();
builder.Services.AddTransient<ILoginUser, LoginUser>();
builder.Services.AddTransient<IRegisterUser, RegisterUser>();
builder.Services.AddTransient<IAssignRoleUser, AssignRoleUser>();
builder.Services.AddTransient<IUnassignRoleUser, UnassignRoleUser>();
builder.Services.AddTransient<IUpdateLoggedUser, UpdateLoggedUser>();
builder.Services.AddTransient<IGetPrincipalUser, GetPrincipalUser>();
builder.Services.AddTransient<IAuthorizationHandler, ProhibitAdminAccountActionHandler>();
builder.Services.AddScoped<IAccessTokenIssuer, AccessTokenIssuer>();

builder.Services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);
var app = builder.Build();

DatabaseSeeder.DatabaseSeed(app.Services);
app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseRouting();
app.UseCors(builder =>
     builder
  .WithOrigins("http://localhost:4200")
  .AllowAnyHeader()
  .AllowCredentials()
  .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints
      .MapControllers()
        .RequireCors(MyAllowSpecificOrigins);
});
app.MapControllers();

app.Run();
