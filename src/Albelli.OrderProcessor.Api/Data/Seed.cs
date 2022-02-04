using Albelli.OrderProcessor.Api.Models;

namespace Albelli.OrderProcessor.Api.Data{
    public static class Seed{
        public static Product[] Products = {
            new Product(1,"photoBook",19f,1),
            new Product(2,"calendar",10f,1),
            new Product(3,"canvas",16f,1),
            new Product(4,"cards",4.7f,1),
            new Product(5,"mug",94f,4)
        };
    }
}