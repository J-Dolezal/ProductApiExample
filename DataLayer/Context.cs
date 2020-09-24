using Microsoft.EntityFrameworkCore;
using ProductApiExample.DataLayer.Entities;

namespace ProductApiExample.DataLayer
{
    public class Context : DbContext
    {
        public const string ConnectionStringName = "ProductApiExampleData";

        public DbSet<Product> Products => Set<Product>();

        public Context(DbContextOptions<Context> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureProductEntity(modelBuilder);
        }

        private static void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Product>();

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Price).HasColumnType("decimal");
            entity.Property(p => p.ImgUri)
                .IsUnicode(false);
            entity.Property(p => p.Name)
                .HasMaxLength(Product.NameMaxLength);
        }
    }
}
