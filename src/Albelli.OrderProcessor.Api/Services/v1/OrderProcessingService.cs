using Microsoft.EntityFrameworkCore;

namespace Albelli.OrderProcessor.Api.Services
{
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
                OrderDto
                {
                    OrderId = x.Id,
                    Items = x.Items.Select(y => new OrderItemDto { Product = y.Product.Name, Quantity = y.Quantity, Width = y.Product.Width }).ToList(),
                    RequiredBinWidth = x.RequiredBinWidth
                }).ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync(string[]? productNames)
        {
            if (productNames != null)
            {
                return await _context.Products.Where(p => productNames.Contains(p.Name)).AsNoTracking().ToListAsync();
            }
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<OrderDto> ProcessOrderAsync(OrderProcessRequest request)
        {
            var producst = await GetProductsAsync(request.OrderItems.Select(oi => oi.Product).ToArray());
            var orderItems = (from p in producst
                              join r in request.OrderItems on p.Name equals r.Product
                              select new OrderItem(p.Id, r.Quantity)).ToList();
            var order = new Order();
            order.Items = orderItems;

            order.RequiredBinWidth = CalculateRequiredBinWidth(orderItems);

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            var response = new OrderDto { OrderId = order.Id, RequiredBinWidth = order.RequiredBinWidth, Items = order.Items.Select(i => new OrderItemDto { Product = i.Product.Name, Quantity = i.Quantity, Width = i.Product.Width }).ToList() };
            return response;
        }
        public decimal CalculateRequiredBinWidth(List<OrderItem> Items)
        {
            if (Items == null)
                return 0m;
            decimal total = 0m;
            var producst = _context.Products.Where(p => Items.Select(x => x.ProductId).Contains(p.Id)).ToList();
            foreach (var item in Items)
            {
                var product = producst.Single(p => p.Id == item.ProductId);
                if (item.Quantity == 1)
                    total += product.Width;
                else
                {
                    var widthCount = item.Quantity / product.StackItemsCount;
                    if (item.Quantity % product.StackItemsCount > 0)
                        widthCount += 1;
                    total += widthCount * product.Width;
                }
            }
            return total;
        }

        public async Task<OrderDto> GetOrderDatailsById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Orders
                .Where(o => o.Id == id)
                .Select(x => new
                OrderDto
                {
                    OrderId = x.Id,
                    Items = x.Items.Select(y => new OrderItemDto { Product = y.Product.Name, Quantity = y.Quantity, Width = y.Product.Width }).ToList(),
                    RequiredBinWidth = x.RequiredBinWidth
                }).SingleOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}