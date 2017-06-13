using System;
using Core.Model.Products;

namespace Core.Model.Orders
{
    public class CustomerOrderEntry
    {
        public CustomerOrderEntry(Product product, decimal unitSalePrice, uint qty = 1)
        {
            // Exception handling
            if (product.Id == null) throw new ArgumentNullException(nameof(product.Id), "Product Id is null");

            ProductId = (uint) product.Id;
            ProductName = product.Name;
            UnitSalePrice = unitSalePrice;
            Qty = qty;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public CustomerOrderEntry()
        {
        }

        public uint ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitSalePrice { get; set; }
        public uint Qty { get; set; }
        public decimal NetPrice => UnitSalePrice * Qty;
    }
}