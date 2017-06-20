using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Model.Customers
{
    public class Order
    {
        private decimal _total;

        public Order(uint? customerId, IEnumerable<OrderItem> orderItems, bool isSettled, bool isCancelled, string note)
        {
            CustomerId = customerId;
            OrderItems = orderItems?.ToList() ?? new List<OrderItem>();
            IsSettled = isSettled;
            IsCancelled = isCancelled;
            Note = note;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public uint? Id { get; set; }

        public decimal Total
        {
            get { return OrderItems?.Sum(oe => oe.NetPrice) ?? _total; }
            set => _total = value;
        }

        public string Note { get; set; }
        public DateTime Date { get; set; }
        public uint? CustomerId { get; set; }
        public uint UserId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsSettled { get; set; }
        public decimal DueAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public void AddOrderEntry(OrderItem orderEntry)
        {
            OrderItems.Add(orderEntry);
        }

        public void RemoveOrderEntry(OrderItem orderEntry)
        {
            OrderItems.Remove(orderEntry);
        }
    }
}