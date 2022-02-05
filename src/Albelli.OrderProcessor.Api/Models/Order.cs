namespace Albelli.OrderProcessor.Api.Models
{
    public class Order
    {
        public Order()
        {
            this.Items = new List<OrderItem>();
        }
        public Order(int id)
        {
            Id = id;
            this.Items = new List<OrderItem>();
        }

        public int Id { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal RequiredBinWidth
        {
            get;
            set;
        }
    }
}