using Domain.Furnitures;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistance
{
    public static class DatabaseSeeder
    {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (context.Database.EnsureCreated())
                {
                    User seededAdmin = new User()    
                    {
                        Username = "Andy",
                        Firstname = "Andrey",
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow,
                        Password ="123456"

                    };

                    User seededUser = new User()
                    {
                        Username = "Ivan",
                        Firstname = "Ivan",
                        IsAdmin = false,
                        CreatedAt = DateTime.UtcNow,
                        Password = "123456"

                    };

                    context.Users.Add(seededAdmin);
                    context.Users.Add(seededUser);

                    Furniture seededFurniture = new Furniture()
                    {
                        Name = "Azure",
                        Description = "Item is suitable for living room",
                        Type = "Sofa"
                    };

                    context.Furnitures.Add(seededFurniture);
                    context.SaveChanges();
                }
            }
        }
    }
}
