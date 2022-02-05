using Albelli.OrderProcessor.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Albelli.OrderProcessor.Api.Data{
    public class OrderProcessorDbContext : DbContext
    {
        public OrderProcessorDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne();
            
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50);

            modelBuilder.Entity<Product>().HasData(Seed.Products);
            modelBuilder.Entity<Order>().HasData(Seed.Orders);            
            modelBuilder.Entity<OrderItem>().HasData(Seed.OrderItems);
            modelBuilder
                .Entity<Order>()
                .Property(o => o.RequiredBinWidth)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
            base.OnModelCreating(modelBuilder);
        }
    }
}