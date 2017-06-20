using Core.Model.Enums;

namespace Core.Model.Products
{
    public abstract class Item : Product
    {
        protected Item(string name, ProductType productType, decimal unitPrice, uint stockQty = 0)
            : base(name, productType)
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

        //NEW PROPERTIES : ITEMCODE, MODEL
        public string ItemCode { get; set; }

        public string Model { get; set; }

        /// <summary>
        ///     Generates and returns a name for item by concatenating defined property values
        ///     Sample : (ItemCode + .... + Brand + Model + ....)
        /// </summary>
        /// <returns></returns>
        public abstract string GenerateName();
    }
}