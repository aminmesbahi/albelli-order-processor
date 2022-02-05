using Albelli.OrderProcessor.Api.Models;

namespace Albelli.OrderProcessor.Api.Data
{
    public static class Seed
    {
        public static Product[] Products = {
            new Product(1,"photoBook",19m,1),
            new Product(2,"calendar",10m,1),
            new Product(3,"canvas",16m,1),
            new Product(4,"cards",4.7m,1),
            new Product(5,"mug",94m,4)
        };

        public static Order[] Orders = {
            new Order(1),
            new Order(2)
            };

        public static OrderItem[] OrderItems = {
            new OrderItem(1,1,1,1),
            new OrderItem(2,1,2,1),
            new OrderItem(3,1,5,1),
            new OrderItem(4,2,4,2),
            new OrderItem(5,2,5,6),
            new OrderItem(6,2,4,2),
            };
    }
}