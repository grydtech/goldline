using System;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Model.Customers
{
    public class OrderItem
    {
        public OrderItem(Product product, decimal unitPrice, uint qty)
        {
            // Exception handling
            if (product?.Id == null) throw new ArgumentNullException(nameof(product.Id), "Product/Id is null");

            Product = product;
            UnitPrice = unitPrice;
            Qty = qty;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public OrderItem()
        {
        }

        public Product Product { get; private set; }
        public decimal UnitPrice { get; set; }
        public uint Qty { get; set; }
        public decimal NetPrice => UnitPrice * Qty;
    }
}