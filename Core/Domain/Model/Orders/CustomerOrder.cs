using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Orders
{
    public class CustomerOrder
    {
        private decimal _total;

        public CustomerOrder(IEnumerable<OrderEntry> orderentries = null, string note = null,
            uint? customerId = null,
            bool isCredit = false)
        {
            OrderEntries = orderentries?.ToList() ?? new List<OrderEntry>();
            Note = note;
            CustomerId = customerId;
            IsCancelled = false;
            IsCredit = isCredit;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public CustomerOrder()
        {
            OrderEntries = new List<OrderEntry>();
        }

        public uint? Id { get; set; }

        public decimal Total
        {
            get { return OrderEntries?.Sum(oe => oe.NetPrice) ?? _total; }
            set => _total = value;
        }

        public string Note { get; set; }
        public DateTime Date { get; set; }
        public uint? CustomerId { get; set; }
        public uint UserId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsCredit { get; set; }
        public decimal DueAmount { get; set; }
        public List<OrderEntry> OrderEntries { get; set; }

        public void AddOrderEntry(OrderEntry orderEntry)
        {
            OrderEntries.Add(orderEntry);
        }

        public void RemoveOrderEntry(OrderEntry orderEntry)
        {
            OrderEntries.Remove(orderEntry);
        }
    }
}