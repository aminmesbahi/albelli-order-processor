namespace Albelli.OrderProcessor.Api.Models
{
    public record OrderDto
    {
        public int OrderId { get; init; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal RequiredBinWidth { get; set; }
    }
    public record OrderItemDto
    {
        public string Product { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public decimal Width { get; set; }
    }
}