using Microsoft.Extensions.DependencyInjection;

namespace Catalogue.API.Persistence
{
    public class DatabaseSeeder
    {
        private static DatabaseContext context;

        public static DatabaseContext Context { get => context; private set => context = value; }

        public static void DatabaseSeed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                Context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                Context.Database.EnsureCreated();
            }
        }
    }
}
