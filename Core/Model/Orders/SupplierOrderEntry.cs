using System;
using Core.Model.Products;

namespace Core.Model.Orders
{
    public class SupplierOrderEntry
    {
        public SupplierOrderEntry(Item item, uint qty, decimal price)
        {
            // Exception handling
            if (item.Id == null) throw new ArgumentNullException(nameof(item.Id), "Item Id is null");

            ItemId = (uint) item.Id;
            ItemName = item.Name;
            Qty = qty;
            Price = price;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public SupplierOrderEntry()
        {
        }

        public uint ItemId { get; set; }
        public string ItemName { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
    }
}