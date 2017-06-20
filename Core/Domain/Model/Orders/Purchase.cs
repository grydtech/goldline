using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model.Enums;

namespace Core.Model.Orders
{
    public class Purchase
    {
        private decimal _total;

        public Purchase(uint supplierId = 0, IEnumerable<PurchasedItem> orderEntries = null,
            decimal amount = 0,
            string note = null,
            SupplyOrderStatus status = SupplyOrderStatus.Pending)
        {
            SupplierId = supplierId;
            OrderEntries = orderEntries?.ToList() ?? new List<PurchasedItem>();
            Amount = amount;
            Note = note;
            Status = status;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Purchase()
        {
            OrderEntries = new List<PurchasedItem>();
        }

        public uint? Id { get; set; }

        public decimal Amount
        {
            get { return OrderEntries?.Sum(oe => oe.Price) ?? _total; }
            set => _total = value;
        }

        public string Note { get; set; }
        public DateTime Date { get; set; }
        public uint SupplierId { get; set; }
        public uint UserId { get; set; }
        public SupplyOrderStatus Status { get; set; }
        public List<PurchasedItem> OrderEntries { get; set; }

        public void AddOrderEntry(PurchasedItem purchasedItem)
        {
            OrderEntries.Add(purchasedItem);
        }

        public void RemoveOrderEntry(PurchasedItem purchasedItem)
        {
            OrderEntries.Remove(purchasedItem);
        }
    }
}