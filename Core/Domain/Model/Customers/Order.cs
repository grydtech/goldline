using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Model.Customers
{
    public class Order
    {
        private decimal _total;

        public Order(uint? customerId, IEnumerable<OrderItem> orderItems, bool isCancelled, string note)
        {
            CustomerId = customerId;
            OrderItems = orderItems?.ToList() ?? new List<OrderItem>();
            DueAmount = Amount;
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
        public uint? CustomerId { get; set; }

        public decimal Amount
        {
            get { return OrderItems?.Sum(oe => oe.NetPrice) ?? _total; }
            set { _total = value; }
        }

        public string Note { get; set; }
        public DateTime Date { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsSettled => DueAmount == 0;
        public decimal DueAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public IEnumerable<OrderPayment> OrderPayments { get; set; }

        public void AddOrderItem(OrderItem orderItem)
        {
            OrderItems.Add(orderItem);
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            OrderItems.Remove(orderItem);
        }
    }
}