using Microsoft.Extensions.DependencyInjection;

namespace Cart.API.Persistence
{
    public class DatabaseSeeder
    {
        public static void DatabaseSeed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
