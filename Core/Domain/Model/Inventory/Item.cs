using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public abstract class Item : Product
    {
        protected Item(ProductType productType, decimal unitPrice, uint stockQty = 0)
            : base(productType)
        {
            StockQty = stockQty;
            UnitPrice = unitPrice;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        protected Item()
        {
        }

        public uint StockQty { get; set; }
        public decimal UnitPrice { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
    }
}