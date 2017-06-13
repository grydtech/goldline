using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model.Enums;

namespace Core.Model.Orders
{
    public class SupplierOrder
    {
        private decimal _total;

        public SupplierOrder(uint supplierId = 0, IEnumerable<SupplierOrderEntry> orderEntries = null,
            decimal total = 0,
            string note = null,
            SupplyOrderStatus status = SupplyOrderStatus.Pending)
        {
            SupplierId = supplierId;
            OrderEntries = orderEntries?.ToList() ?? new List<SupplierOrderEntry>();
            Total = total;
            Note = note;
            Status = status;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public SupplierOrder()
        {
            OrderEntries = new List<SupplierOrderEntry>();
        }

        public uint? Id { get; set; }

        public decimal Total
        {
            get { return OrderEntries?.Sum(oe => oe.Price) ?? _total; }
            set => _total = value;
        }

        public string Note { get; set; }
        public DateTime Date { get; set; }
        public uint SupplierId { get; set; }
        public uint UserId { get; set; }
        public SupplyOrderStatus Status { get; set; }
        public List<SupplierOrderEntry> OrderEntries { get; set; }

        public void AddOrderEntry(SupplierOrderEntry supplierOrderEntry)
        {
            OrderEntries.Add(supplierOrderEntry);
        }

        public void RemoveOrderEntry(SupplierOrderEntry supplierOrderEntry)
        {
            OrderEntries.Remove(supplierOrderEntry);
        }
    }
}