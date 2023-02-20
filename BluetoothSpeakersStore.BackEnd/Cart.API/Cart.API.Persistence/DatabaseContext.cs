using Microsoft.EntityFrameworkCore;
using Cart.API.Domain;

namespace Cart.API.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DatabaseContext() : base()
        {

        }

        public virtual DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> Carts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CartProductSetUp(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void CartProductSetUp(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasKey(p => p.CartId);
            modelBuilder.Entity<Product>().HasKey(p => p.InCartId);   
            modelBuilder.Entity<ShoppingCart>().HasKey(p => p.CartId);
            modelBuilder.Entity<ShoppingCart>()
                  .HasMany(prop => prop.Products)
                      .WithMany(prop => prop.Carts)
                          .UsingEntity(j => j.ToTable("CartsProducts"));
        }
    }
}
