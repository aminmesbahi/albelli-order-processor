using Albelli.OrderProcessor.Api.Data;
using Albelli.OrderProcessor.Api.Models;
using Albelli.OrderProcessor.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albelli.OrderProcessor.Test;

public class OrderProcessorTests
{
    [Fact]
    public void Calculate_min_bin_width_for_simple_order()
    {        
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req1 = new List<OrderItem>{ new OrderItem (1, 1) };

        //Act
        var res1 = service.CalculateRequiredBinWidth(req1);

        //Assert
        Assert.Equal(19m, res1);
    }

    [Fact]
    public void Calculate_min_bin_width_for_simple_repeated_multiple_orders()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req2 = new List<OrderItem> { new OrderItem(1, 1), new OrderItem(1, 2) };

        //Act
        var res2 = service.CalculateRequiredBinWidth(req2);

        //Assert
        Assert.Equal(57m, res2);
    }

    [Fact]
    public void Calculate_min_bin_width_for_simple_multiple_orders()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req3 = new List<OrderItem> { new OrderItem(1, 1), new OrderItem(5, 1), new OrderItem(2, 2) };

        //Act
        var res3 = service.CalculateRequiredBinWidth(req3);

        //Assert
        Assert.Equal(133m, res3);
    }

    [Fact]
    public void Calculate_min_bin_width_for_stackable_item_bordered_quantity()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req4 = new List<OrderItem> { new OrderItem(1, 1), new OrderItem(5, 4) };

        //Act
        var res4 = service.CalculateRequiredBinWidth(req4);

        //Assert
        Assert.Equal(113m, res4);
    }

    [Fact]
    public void Calculate_min_bin_width_for_stackable_item_beyound_one_stack_in_multiple_records()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req5 = new List<OrderItem> { new OrderItem(5, 1), new OrderItem(5, 4) };

        //Act
        var res5 = service.CalculateRequiredBinWidth(req5);

        //Assert
        Assert.Equal(188m, res5);
    }
    
    [Fact]
    public void Calculate_min_bin_width_for_mutltiple_stacks_of_items()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req6 = new List<OrderItem> { new OrderItem(5, 5), new OrderItem(5, 4), new OrderItem(3, 4) };

        //Act
        var res6 = service.CalculateRequiredBinWidth(req6);

        //Assert
        Assert.Equal(346m, res6);
    }

    [Fact]
    public void Calculate_min_bin_width_for_mutltiple_stacks_of_items_beyond_stack_quanitity()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req7 = new List<OrderItem> { new OrderItem(5, 1), new OrderItem(5, 2), new OrderItem(5, 2) };

        //Act
        var res7 = service.CalculateRequiredBinWidth(req7);

        //Assert
        Assert.Equal(188m, res7);
    }

    [Fact]
    public void Calculate_min_bin_for_combination_of_stackable_and_simple()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req8 = new List<OrderItem> { new OrderItem(5, 3), new OrderItem(5, 2), new OrderItem(4, 1) };

        //Act
        var res8 = service.CalculateRequiredBinWidth(req8);

        //Assert
        Assert.Equal(192.7m, res8);
    }

    [Fact]
    public void Calculate_min_bin_width_for_simple_order_with_floating_ponit()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req9 = new List<OrderItem> { new OrderItem(4, 1) };

        //Act
        var res9 = service.CalculateRequiredBinWidth(req9);

        //Assert
        Assert.Equal(4.7m, res9);
    }

    [Fact]
    public void Calculate_min_bin_width_for_empty_list_of_items()
    {
        //Arrange
        var service = new OrderProcessingServiceFake();
        var req10 = new List<OrderItem> { };

        //Act
        var res10 = service.CalculateRequiredBinWidth(req10);

        //Assert
        Assert.Equal(0m, res10);
    }
}
public class OrderProcessingServiceFake : IOrderProcessingService
{
    private Mock<IOrderProcessorDbContext> _context;
    private List<Product> _products;
    private List<Order> _orders;
    private List<OrderItem> _orderItems;
 
    public decimal CalculateRequiredBinWidth(List<OrderItem> Items)
    {
        //Setup
        var _context = new Mock<IOrderProcessorDbContext>();
        _products = new List<Product>(Seed.Products);
        _orders = new List<Order>(Seed.Orders);
        _orderItems = new List<OrderItem>(Seed.OrderItems);
        _context.Setup(x => x.Products).ReturnsDbSet(_products);
        _context.Setup(x => x.Orders).ReturnsDbSet(_orders);
        _context.Setup(x => x.OrderItems).ReturnsDbSet(_orderItems);

        Items = Items.GroupBy(i => i.ProductId).Select(g => new OrderItem(g.Key, g.Sum(i => i.Quantity))).ToList();

        if (Items == null)
            return 0m;
        decimal total = 0m;
        var producst = _context.Object.Products.Where(p => Items.Select(x => x.ProductId).Contains(p.Id)).ToList();
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

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        return await _context.Object.Orders
                .Select(x => new
                OrderDto
                {
                    OrderId = x.Id,
                    Items = x.Items.Select(y => new OrderItemDto { Product = y.Product.Name, Quantity = y.Quantity, Width = y.Product.Width }).ToList(),
                    RequiredBinWidth = x.RequiredBinWidth
                }).ToListAsync();
    }

    public async Task<OrderDto> GetOrderDatailsById(int id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Object.Orders
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

    public async Task<List<Product>> GetProductsAsync(string[]? productNames)
    {
        if (productNames != null)
        {
            return await _context.Object.Products.Where(p => productNames.Contains(p.Name)).AsNoTracking().ToListAsync();
        }
        return await _context.Object.Products.AsNoTracking().ToListAsync();
    }

    public Task<OrderDto> ProcessOrderAsync(OrderProcessRequest request)
    {
        throw new System.NotImplementedException();
    }
}