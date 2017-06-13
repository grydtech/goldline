using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Orders
{
    public class CustomerOrder
    {
        private decimal _total;

        public CustomerOrder(IEnumerable<CustomerOrderEntry> orderentries = null, string note = null,
            uint? customerId = null,
            bool isCredit = false)
        {
            OrderEntries = orderentries?.ToList() ?? new List<CustomerOrderEntry>();
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
            OrderEntries = new List<CustomerOrderEntry>();
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
        public List<CustomerOrderEntry> OrderEntries { get; set; }

        public void AddOrderEntry(CustomerOrderEntry customerOrderEntry)
        {
            OrderEntries.Add(customerOrderEntry);
        }

        public void RemoveOrderEntry(CustomerOrderEntry customerOrderEntry)
        {
            OrderEntries.Remove(customerOrderEntry);
        }
    }
}