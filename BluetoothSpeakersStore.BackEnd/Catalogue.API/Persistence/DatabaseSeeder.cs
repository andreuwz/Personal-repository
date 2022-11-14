namespace Catalogue.API.Persistence
{
    internal class DatabaseSeeder
    {
        private static DatabaseContext context;

        internal static DatabaseContext Context { get => context; private set => context = value; }

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
