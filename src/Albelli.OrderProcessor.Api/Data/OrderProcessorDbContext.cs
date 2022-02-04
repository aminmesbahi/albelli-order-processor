using Albelli.OrderProcessor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Albelli.OrderProcessor.Api.Data{
    public class OrderProcessorDbContext : DbContext
    {
        protected OrderProcessorDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50);

            modelBuilder.Entity<Product>().HasData(Seed.Products);
            base.OnModelCreating(modelBuilder);
        }
    }
}