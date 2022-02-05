namespace Albelli.OrderProcessor.Api.Models
{
    public class Product
    {
        public Product()
        {

        }
        public Product(int id, string name, decimal width, int stackItemsCount)
        {
            Id = id;
            Name = name;
            Width = width;
            StackItemsCount = stackItemsCount;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Width { get; set; }
        public int StackItemsCount { get; set; }
    }
}