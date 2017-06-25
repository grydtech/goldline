using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Model.Suppliers
{
    public class Purchase
    {
        public Purchase(uint? supplierId, IEnumerable<PurchaseItem> orderEntries, decimal amount, string note, bool isSettled)
        {
            SupplierId = supplierId;
            PurchaseItems = orderEntries?.ToList() ?? new List<PurchaseItem>();
            Amount = amount;
            Note = note;
            IsSettled = isSettled;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Purchase()
        {
            PurchaseItems = new List<PurchaseItem>();
        }

        public uint? Id { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public uint? SupplierId { get; set; }
        public bool IsSettled { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; }

        public void AddOrderEntry(PurchaseItem purchasedItem)
        {
            PurchaseItems.Add(purchasedItem);
        }

        public void RemoveOrderEntry(PurchaseItem purchasedItem)
        {
            PurchaseItems.Remove(purchasedItem);
        }
    }
}