using Albelli.OrderProcessor.Api.Models;

namespace Albelli.OrderProcessor.Api.Services
{
    public interface IOrderProcessingService
    {
        public Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        public Task<OrderDto> ProcessOrderAsync(OrderProcessRequest request);
        public Task<List<Product>> GetProductsAsync(string[]? productNames);
        public decimal CalculateRequiredBinWidth(List<OrderItem> Items);
    }
}