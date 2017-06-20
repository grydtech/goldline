using System;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Model.Suppliers
{
    public class PurchaseItem
    {
        public PurchaseItem(Item item, uint qty, decimal price)
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
        public PurchaseItem()
        {
        }

        public uint ItemId { get; set; }
        public string ItemName { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
    }
}