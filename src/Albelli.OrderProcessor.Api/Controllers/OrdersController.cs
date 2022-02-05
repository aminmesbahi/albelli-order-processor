using Albelli.OrderProcessor.Api.Models;
using Albelli.OrderProcessor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Albelli.OrderProcessor.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderProcessingService _service;
        public OrdersController(IOrderProcessingService service)
        {
            _service = service;
        }
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [HttpGet("GetOrdersList")]
        public async Task<IEnumerable<OrderDto>> GetAsync(CancellationToken cancellationToken)
        {
            return await _service.GetAllOrdersAsync();
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [HttpPost("ProcessOrder")]
        public async Task<OrderDto> ProcessOrder(OrderProcessRequest request, CancellationToken cancellationToken)
        {
            return await _service.ProcessOrderAsync(request);
        }
    }
}