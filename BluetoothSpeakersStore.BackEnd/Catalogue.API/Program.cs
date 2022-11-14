using Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct;
using Catalogue.API.Application.ShopProduct.Commands.CreateProduct;
using Catalogue.API.Application.ShopProduct.Commands.RemoveProduct;
using Catalogue.API.Application.ShopProduct.Commands.UpdateProduct;
using Catalogue.API.Application.ShopProduct.Queries.GetAllProducts;
using Catalogue.API.Application.ShopProduct.Queries.GetAllProductsAdmin;
using Catalogue.API.Application.ShopProduct.Queries.GetProduct;
using Catalogue.API.Application.ShopProduct.Queries.GetProductAdmin;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Persistence;
using Catalogue.API.Persistence.Repository;
using Catalogue.API.Web.AsyncMessageBusServices;
using Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages;
using Catalogue.API.Web.AsyncMessageBusServices.PublishMessages;
using Catalogue.API.Web.AsyncMessageBusServices.Rpc;
using Catalogue.API.Web.EventProcessing;
using Catalogue.API.Web.Middleware;
using Catalogue.API.Web.TransientFaultsPolicies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<TransientFaultPolicy>();

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IGetProduct, GetProduct>();
builder.Services.AddTransient<IGetProductAsAdmin, GetProductAsAdmin>();
builder.Services.AddTransient<IGetAllProductsAsAdmin, GetAllProductsAsAdmin>();
builder.Services.AddTransient<IProductUpdate, ProductUpdate>();
builder.Services.AddTransient<IGetAllProducts, GetAllProducts>();
builder.Services.AddTransient<IProductRemove, ProductRemove>();
builder.Services.AddTransient<IProductValidations, ProductValidations>();
builder.Services.AddTransient<IProductCreate, ProductCreate>();
builder.Services.AddTransient<IAddProductToCart, AddProductToCart>();

builder.Services.AddSingleton<ConnectionState>(); 
builder.Services.AddTransient<IPublishNewMessage, PublishNewMessage>(); 
builder.Services.AddTransient<IUpdatedUserInfo, UpdatedUserInfo>(); 
builder.Services.AddTransient<IBuyProduct, BuyProduct>(); 
builder.Services.AddSingleton<SessionState>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>(); //background service
builder.Services.AddHostedService<RpcServer>(); //background service

builder.Services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header// and configures them to validate the token with the options.AddJwtBearer(options =>
.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:7268";
    options.Audience = "https://localhost:7268/resources";
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
