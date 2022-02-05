namespace Albelli.OrderProcessor.Api.Models{
    public record OrderDto{
        public int OrderId { get; init; }
        public IEnumerable<OrderItemDto> Items { get; set; } 
        public decimal RequiredBinWidth{get; set;} 
    }
    public record OrderItemDto{
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Width { get; set; }
    }
}