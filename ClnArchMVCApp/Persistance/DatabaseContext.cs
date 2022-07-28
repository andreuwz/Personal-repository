using Domain.Furnitures;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Furniture> Furnitures { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupFurnitureConfiguration(modelBuilder);
            SetupUserConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void SetupFurnitureConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Furniture>().HasKey(p => p.Id);
            modelBuilder.Entity<Furniture>().Property(p => p.Name).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<Furniture>().Property(p => p.Type).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<Furniture>().Property(p => p.Description).IsRequired().HasMaxLength(120);
         
        }

        private static void SetupUserConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().Property(p => p.Firstname).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(p => p.Username).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<User>().HasIndex(p => p.Username).IsUnique();
            modelBuilder.Entity<User>().Property(p => p.IsAdmin).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Password).IsRequired().HasMaxLength(20);
           
        }
    }
}
