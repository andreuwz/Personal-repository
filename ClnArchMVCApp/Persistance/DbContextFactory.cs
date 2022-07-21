using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistance
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=FurnitureShopDB;Trusted_Connection=True;");

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
