using System;
using Core.Model.Products;

namespace Core.Model.Orders
{
    public class OrderEntry
    {
        public OrderEntry(Product product, decimal unitPrice, uint qty = 1)
        {
            // Exception handling
            if (product.Id == null) throw new ArgumentNullException(nameof(product.Id), "Product Id is null");

            ProductId = (uint) product.Id;
            ProductName = product.Name;
            UnitPrice = unitPrice;
            Qty = qty;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public OrderEntry()
        {
        }

        public uint ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public uint Qty { get; set; }
        public decimal NetPrice => UnitPrice * Qty;
    }
}