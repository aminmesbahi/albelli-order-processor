using Albelli.OrderProcessor.Api.Models;

namespace Albelli.OrderProcessor.Api.Services{
    public interface IOrderProcessingService{
        public Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    }
}