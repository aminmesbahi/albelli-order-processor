namespace Albelli.OrderProcessor.Api.Models
{
    public class OrderItemProcessRequest
    {
        public OrderItemProcessRequest(string product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public string Product { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderProcessRequest
    {
        public List<OrderItemProcessRequest> OrderItems { get; set; } = default!;
    }
}