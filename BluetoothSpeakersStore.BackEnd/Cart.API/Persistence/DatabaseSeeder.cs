namespace Cart.API.Persistence
{
    internal class DatabaseSeeder
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
