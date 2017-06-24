using System;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Model.Customers
{
    public class OrderItem
    {
        public OrderItem(uint productId, string productName, uint qty, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Qty = qty;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public OrderItem()
        {
        }

        public uint ProductId { get; private set; }
        public string ProductName { get; private set; }
        public uint Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetPrice => UnitPrice * Qty;
    }
}