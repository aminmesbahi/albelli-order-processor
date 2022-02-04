namespace Albelli.OrderProcessor.Api.Models
{
    public class Product
    {
        public Product(int id, string name, float width, int stackItemsCount)
        {
            Id = id;
            Name = name;
            Width = width;
            StackItemsCount = stackItemsCount;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public int StackItemsCount { get; set; }
    }
}