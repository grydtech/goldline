using System;

namespace Core.Domain.Model.Suppliers
{
    public class PurchaseItem
    {
        public PurchaseItem(uint itemId, string itemName, uint qty)
        {
            if (itemName == null) throw new ArgumentNullException(nameof(itemName), "Item Name is null");
            ItemId = itemId;
            ItemName = itemName;
            Qty = qty;
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
    }
}