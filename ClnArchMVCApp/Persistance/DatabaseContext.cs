using Domain.Furnitures;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class DatabaseContext : DbContext
    {
        public IServiceProvider applicationServices;

        public IServiceProvider ApplicationSettings { get; set; }

        public virtual DbSet<Furniture> Furnitures { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
        }

       
    }
}
