namespace Albelli.OrderProcessor.Api.Models{
    public class Order{
        public Order(int id, IEnumerable<OrderItem> items)
        {
            Id = id;
            Items = items;
        }

        public int Id { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }     
    }
}