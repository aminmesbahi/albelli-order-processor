using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Albelli.OrderProcessor.Api.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderProcessingService _service;
        public OrdersController(IOrderProcessingService service)
        {
            _service = service;
        }
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        [HttpGet("GetOrdersList")]
        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(CancellationToken cancellationToken)
        {
            return await _service.GetAllOrdersAsync();
        }
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [HttpGet("GetOrderById")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            if(orderId < 1)
            {
                return BadRequest();
            }
            var result= await _service.GetOrderDatailsById(orderId);
            if (result== null)
                return NotFound();
            else
                return Ok(result);
        }

        /// <summary>
        /// Process an Order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Created order with order item details and calculate required bin width</returns>
        /// <remarks>
        /// Sample request:
        ///     
        /// Current valid products: photoBook, calendar, canvas, cards, mug
        ///     
        ///     {
        ///         "orderItems": [
        ///           {
        ///             "product": "photoBook",
        ///             "quantity": 2
        ///           },
        ///           {
        ///             "product": "calendar",
        ///             "quantity": 2
        ///           },
        ///           {
        ///           "product": "canvas",
        ///             "quantity": 2
        ///           }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the order, details and Calculate Required Bin Width</response>
        /// <response code="400">If oder items is null or invalid</response>
        [SwaggerOperation("Accepts an order, stores it, and responds with the minimum bin width")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("ProcessOrder")]
        public async Task<ActionResult<OrderDto>> ProcessOrder(OrderProcessRequest request, CancellationToken cancellationToken)
        {
            if (request.OrderItems.Count > 0)
            {
                request.OrderItems = request.OrderItems.GroupBy(i => i.Product).Select(g => new OrderItemProcessRequest(g.Key, g.Sum(i => i.Quantity))).ToList();
                return await _service.ProcessOrderAsync(request);
            }
            else
                return BadRequest();            
        }
    }
}