namespace Core.Domain.Model.Inventory
{
    public abstract class Item : Product
    {
        protected Item(string brand, string model, uint stockQty, decimal unitPrice)
        {
            StockQty = stockQty;
            UnitPrice = unitPrice;
            Brand = brand;
            Model = model;
        }

        protected Item()
        {   
        }

        public abstract bool Validate();
        public string Brand { get; set; }
        public string Model { get; set; }
        public uint StockQty { get; set; }
        public decimal UnitPrice { get; set; }
    }
}