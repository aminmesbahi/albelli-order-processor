namespace Albelli.OrderProcessor.Api.Models
{
    public class OrderItem
    {
        public OrderItem(int id, int orderId, int productId, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
        public OrderItem(int productId, Product product, int quantity)
        {
            ProductId = productId;
            Product = product;
            Quantity = quantity;
        }
        public OrderItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}