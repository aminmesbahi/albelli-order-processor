namespace Albelli.OrderProcessor.Api.Models{
    public class OrderItem{
        public OrderItem(int id, Product product, int quantity)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}