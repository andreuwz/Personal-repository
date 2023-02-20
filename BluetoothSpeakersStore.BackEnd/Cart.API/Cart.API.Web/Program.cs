using AutoMapper.EquivalencyExpression;
using Cart.API.Application.Commands.CheckoutCart;
using Cart.API.Application.Commands.RemoveCart;
using Cart.API.Application.Commands.RemoveLoggedUserCart;
using Cart.API.Application.Commands.RemoveProductFromCart;
using Cart.API.Application.Queries.GetCartProducts;
using Cart.API.Application.Queries.GetCurrentUserCart;
using Cart.API.Application.Queries.GetCurrentUserCartProducts;
using Cart.API.Application.Queries.GetUserCart;
using Cart.API.Application.Validations;
using Cart.API.Persistence;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices;
using Cart.API.Web.AsyncMessageBusServices.EventProcessing;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts;
using Cart.API.Web.AsyncMessageBusServices.PublishMessages;
using Cart.API.Web.AsyncMessageBusServices.Rpc;
using Cart.API.Web.Middleware;
using Cart.API.Web.TransientFaultsPolicies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

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
        });
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper((serviceProvider, automapper) =>
{
    automapper.AddCollectionMappers();

}, typeof(Program));

builder.Services.AddSingleton<TransientFaultPolicy>();

builder.Services.AddTransient<IAddProductToShoppingCart, AddProductToShoppingCart>();
builder.Services.AddSingleton<ConnectionState>();
builder.Services.AddSingleton<SessionState>();
builder.Services.AddTransient<IPublishNewMessage, PublishNewMessage>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddTransient<IUpdatedUserInfo, UpdatedUserInfo>();
builder.Services.AddTransient<IDeleteUserInfo, DeleteUserInfo>();
builder.Services.AddTransient<IUpdateProductInfo, UpdateProductInfo>();
builder.Services.AddTransient<IDeleteProductFromCart, DeleteProductFromCart>();
builder.Services.AddHostedService<MessageBusSubscriber>(); //background service
builder.Services.AddScoped<IProductRpcClient, ProductRpcClient>();
builder.Services.AddScoped<IUserRpcClient, UserRpcClient>();
builder.Services.AddTransient<IRpcHelperService, RpcHelperService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartValidations, CartValidations>();
builder.Services.AddTransient<IGetShoppingCart, GetShoppingCart>();
builder.Services.AddTransient<IGetLoggedUserProductsInCart, GetLoggedUserProductsInCart>();
builder.Services.AddTransient<IRemoveShoppingCart, RemoveShoppingCart>();
builder.Services.AddTransient<ICheckoutShoppingCart, CheckoutShoppingCart>();
builder.Services.AddTransient<IRemoveLoggedUserShoppingCart, RemoveLoggedUserShoppingCart>();
builder.Services.AddTransient<IRemoveProductInCart, RemoveProductInCart>();
builder.Services.AddTransient<IGetLoggedUserCart, GetLoggedUserCart>();
builder.Services.AddTransient<IGetCartOfUser, GetCartOfUser>();

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy =>
                    policy.RequireRole("Administrator"));

    options.AddPolicy("MasterAdmin", policy =>
                    policy.RequireRole("MasterAdmin"));

    options.AddPolicy("RegularUser", policy =>
                    policy.RequireRole("RegularUser"));
});

var app = builder.Build();

DatabaseSeeder.DatabaseSeed(app.Services);
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
