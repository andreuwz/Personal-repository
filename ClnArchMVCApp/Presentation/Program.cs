using Application.Furnitures.Commands.AddFurniture.FurnitureFactory;
using Application.Furnitures.Commands.RemoveFurniture;
using Application.Furnitures.Commands.UpdateFurniture;
using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetSingleFurniture;
using Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddTransient<IFurnitureRepository, FurnitureRepository>();
builder.Services.AddTransient<IRemoveFurniture, RemoveFurniture>();
builder.Services.AddTransient<IUpdateFurniture, UpdateFurniture>();
builder.Services.AddTransient<IFurnitureFactory, FurnitureFactory>();
builder.Services.AddTransient<IGetAllFurnituresListQuery, GetAllFurnituresQuery>();
builder.Services.AddTransient<IGetSingleFurnitureQuery, GetSingleFurnitureQuery>();



var app = builder.Build();

DatabaseSeeder.Seed(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Furniture}/{action=Index}/{id?}");

app.Run();


    
