namespace Albelli.OrderProcessor.Api.Models{
    public class Order{
        public Order()
        {
            
        }
        public Order(int id)
        {
            Id = id;
            this.Items=new List<OrderItem>();
        }

        public int Id { get; set; }
        public List<OrderItem> Items { get; set; }  
        public decimal RequiredBinWidth {
        get => CalculateRequiredBinWidth(); 
        private set => _requiredBinWidth = CalculateRequiredBinWidth(); }
        private decimal _requiredBinWidth;
        public decimal CalculateRequiredBinWidth(){
            if(Items==null)
                return 0m;
            decimal total=0m;
            foreach (var item in Items)
            {
                if(item.Quantity==1)
                    total+=item.Product.Width;
                else{
                    var widthCount=item.Quantity/item.Product.StackItemsCount;
                    if(item.Quantity%item.Product.StackItemsCount>0)
                        widthCount+=1;
                    total+=widthCount*item.Product.Width;
                }                
            }
            return total;
        }
    }
}