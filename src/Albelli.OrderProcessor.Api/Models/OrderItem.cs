namespace Albelli.OrderProcessor.Api.Models{
    public class OrderItem{
        public OrderItem(int id, int orderId, int productId, int quantity)
        {
            Id = id;
            OrderId=orderId;
            ProductId=productId;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}