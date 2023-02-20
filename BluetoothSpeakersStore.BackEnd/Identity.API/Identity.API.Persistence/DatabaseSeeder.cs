using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Persistence
{
    public class DatabaseSeeder
    {
        public static void DatabaseSeed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (context.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    User initialAdmin = new User()
                    {
                        UserName = "Andrey@abv.bg",
                        Firstname = "Andrey",
                        Lastname = "Dimitrov",
                        Id = Guid.NewGuid().ToString("D"),
                        Email = "Andrey@abv.bg",
                        NormalizedEmail = "Andrey@abv.bg".ToUpper(),
                        EmailConfirmed = true,
                        NormalizedUserName = "Andrey@abv.bg".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        Balance = 100000
                    };

                    initialAdmin.PasswordHash = hasher.HashPassword(initialAdmin, "adminpass");

                    IdentityRole adminRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Administrator",
                        NormalizedName = "Administrator".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityRole userRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "RegularUser",
                        NormalizedName = "RegularUser".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityRole masterAdminRole = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "MasterAdmin",
                        NormalizedName = "MasterAdmin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };


                    IdentityUserRole<string> initialAdminRole = new IdentityUserRole<string>()
                    {
                        RoleId = adminRole.Id,
                        UserId = initialAdmin.Id
                    };

                    IdentityUserRole<string> masterAdmin = new IdentityUserRole<string>()
                    {
                        RoleId = masterAdminRole.Id,
                        UserId = initialAdmin.Id
                    };

                    context.Users.Add(initialAdmin);
                    context.Roles.Add(adminRole);
                    context.Roles.Add(userRole);
                    context.Roles.Add(masterAdminRole);
                    context.UserRoles.Add(initialAdminRole);
                    context.UserRoles.Add(masterAdmin);
                    context.SaveChanges();
                }
            }
        }
    }
}
