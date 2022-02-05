using Albelli.OrderProcessor.Api.Data;
using Albelli.OrderProcessor.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Albelli.OrderProcessor.Api.Services{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly OrderProcessorDbContext _context;
        public OrderProcessingService(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<OrderProcessorDbContext>();
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders 
    .Select(x => new 
    OrderDto {
        OrderId = x.Id, //whatever
        Items = x.Items.Select(y => new OrderItemDto{Product=y.Product.Name, Quantity=y.Quantity,Width=y.Product.Width}).ToArray(),
        RequiredBinWidth=x.RequiredBinWidth
    }).ToListAsync();

        }
    }
}