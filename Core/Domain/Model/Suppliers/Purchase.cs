using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Enums;

namespace Core.Domain.Model.Suppliers
{
    public class Purchase
    {
        private decimal _total;

        public Purchase(uint supplierId = 0, IEnumerable<PurchaseItem> orderEntries = null,
            decimal amount = 0,
            string note = null,
            SupplyOrderStatus status = SupplyOrderStatus.Pending)
        {
            SupplierId = supplierId;
            OrderEntries = orderEntries?.ToList() ?? new List<PurchaseItem>();
            Amount = amount;
            Note = note;
            Status = status;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Purchase()
        {
            OrderEntries = new List<PurchaseItem>();
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
        public List<PurchaseItem> OrderEntries { get; set; }

        public void AddOrderEntry(PurchaseItem purchasedItem)
        {
            OrderEntries.Add(purchasedItem);
        }

        public void RemoveOrderEntry(PurchaseItem purchasedItem)
        {
            OrderEntries.Remove(purchasedItem);
        }
    }
}