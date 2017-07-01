using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Data;
using Core.Data.Customers;
using Core.Data.Inventory;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Handlers
{
    public class OrderHandler
    {
        /// <summary>
        ///     Adds a new Order
        /// </summary>
        public void AddOrder(Order order)
        {
            // Exception handling
            if (order.OrderItems == null)
                throw new ArgumentNullException(nameof(order.OrderItems),
                    "Attempted inserting order without orderEntries initialized");
            if (order.IsCancelled)
                throw new ArgumentException("Attempted inserting a cancelled order", nameof(order.IsCancelled));

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var orderDal = new OrderDal(connection);
                    var orderItemDal = new OrderItemDal(connection);
                    var itemDal = new ItemDal(connection);

                    // Insert order record
                    orderDal.Insert(order.CustomerId, order.Amount, order.Note);
                    order.Id = orderDal.GetLastInsertId();
                    if (order.Id == null)
                        throw new ArgumentNullException(nameof(order.Id),
                            "Order has not been set after insert");

                    // Insert order entries
                    orderItemDal.InsertMultiple(order.Id.Value, order.OrderItems);
                    foreach (var orderItem in order.OrderItems)
                    {
                        itemDal.Update(orderItem.ProductId, stockIncrement:((int)-orderItem.Qty));
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Searches for all orders and returns matches
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Order> GetOrders(bool? isCredit = null, uint? customerId = null, string note = null)
        {
            using (var connection = Connector.GetConnection())
            {
                var orderDal = new OrderDal(connection);
                var orderItemDal = new OrderItemDal(connection);
                var orders = orderDal.Search(isCredit, customerId, note == null ? null : $"%{note}%");
                foreach (var order in orders)
                {
                    order.OrderItems = orderItemDal.Search(order.Id).ToList();
                    yield return order;
                }
            }
        }

        /// <summary>
        ///     Loads OrderItems into passed Order
        /// </summary>
        /// <param name="order"></param>
        public void LoadOrderItems(Order order)
        {
            if (order.Id == null) throw new ArgumentNullException(nameof(order.Id), "Order Id is null");
            using (var connection = Connector.GetConnection())
            {
                var orderItemDal = new OrderItemDal(connection);
                order.OrderItems = orderItemDal.Search(order.Id).ToList();
            }
        }

        /// <summary>
        ///     Updates the customer order
        /// </summary>
        /// <param name="order"></param
        /// <param name="orderItems"></param>
        /// <param name="customerId"></param>
        /// <param name="note"></param>
        /// <param name="isCancelled"></param>
        public void UpdateOrder(Order order, IEnumerable<OrderItem> orderItems = null, uint? customerId = null,
            string note = null, bool? isCancelled = null)
        {
            // Exception handling
            if (order.Id == null)
                throw new ArgumentNullException(nameof(order.Id),
                    "Order Id is null");
            if (order.OrderItems == null)
                throw new ArgumentNullException(nameof(order.OrderItems),
                    "Order OrderItems not loaded");

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    new OrderItemDal(connection).InsertMultiple(order.Id.Value, orderItems);
                    new OrderDal(connection).Update(order.Id.Value, customerId, note, isCancelled);
                }
                scope.Complete();
            }
        }
    }
}