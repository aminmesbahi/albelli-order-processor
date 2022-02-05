using Microsoft.EntityFrameworkCore;

namespace Albelli.OrderProcessor.Api.Data
{
    public interface IOrderProcessorDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; } 
    }
}
