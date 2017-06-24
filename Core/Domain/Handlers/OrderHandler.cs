using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Data;
using Core.Data.Customers;
using Core.Data.Suppliers;
using Core.Domain.Enums;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Employees;
using Core.Domain.Model.Suppliers;

namespace Core.Domain.Handlers
{
    public class OrderHandler
    {
        /// <summary>
        ///     Adds a new Order
        /// </summary>
        public void AddCustomerOrder(Order order)
        {
            // Exception handling
            if (order.OrderItems == null)
                throw new ArgumentNullException(nameof(order.OrderItems),
                    "Attempted inserting order without orderEntries initialized");
            if (order.IsSettled && order.CustomerId == null)
                throw new ArgumentException("Attempted inserting creditorder without customerId",
                    nameof(order.CustomerId));
            if (order.IsCancelled)
                throw new ArgumentException("Attempted inserting a cancelled order", nameof(order.IsCancelled));

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var customerOrderDal = new OrderDal(connection);

                    // Insert order record
                    customerOrderDal.Insert(order);
                    if (order.Id == null)
                        throw new ArgumentNullException(nameof(order.Id),
                            "Order has not been set after insert");

                    // Insert order entries
                    customerOrderDal.InsertOrderItems((uint) order.Id, order.OrderItems);

                    // Insert as credit order if IsSettled true
                    if (order.IsSettled)
                        customerOrderDal.InsertAsCreditOrder((uint) order.Id, (uint) order.CustomerId);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Gets a list of most recent orders.
        ///     The order items are also loaded at this point if isOrderEntriesLoaded is given true
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Order> GetRecentCustomerOrders(bool isOrderEntriesLoaded = true,
            bool isLimited = true, bool? isCredit = null)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var customerOrderDal = new OrderDal(connection);
                    var orders = customerOrderDal.Get(
                        isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit, isCredit);

                    if (!isOrderEntriesLoaded)
                        foreach (var customerOrder in orders)
                            yield return customerOrder;
                    else
                        foreach (var customerOrder in orders)
                        {
                            customerOrderDal.GetOrderItems(customerOrder);
                            yield return customerOrder;
                        }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Gets records of customer orders of given customer.
        ///     The order items are also loaded at this point.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isLimited">Default limit is 20. Make this false to get complete list</param>
        /// <returns></returns>
        public IEnumerable<Order> GetCustomerOrdersOf(Customer customer, bool isLimited = true)
        {
            // Exception handling
            if (customer.Id == null)
                throw new ArgumentNullException(nameof(customer.Id), "The customer Id of customer object is null");

            using (var connection = Connector.GetConnection())
            {
                return new OrderDal(connection).GetOrders((uint) customer.Id,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }

        /// <summary>
        ///     Returns list of orders matching the given note text
        /// </summary>
        /// <param name="note"></param>
        /// <param name="isOrderEntriesLoaded"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetCustomerOrders(string note, bool isOrderEntriesLoaded = true)
        {
            // Exception handling
            if (note == null) throw new ArgumentNullException(nameof(note), "The note to search for is null");

            using (var connection = Connector.GetConnection())
            {
                var customerOrderDal = new OrderDal(connection);
                var orders = customerOrderDal.GetOrders(note);

                if (!isOrderEntriesLoaded)
                    foreach (var customerOrder in orders)
                        yield return customerOrder;
                else
                    foreach (var customerOrder in orders)
                    {
                        customerOrderDal.GetOrderItems(customerOrder);
                        yield return customerOrder;
                    }
            }
        }

        /// <summary>
        ///     Updates the customer order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isEntriesUpdated">If order entries should be updated set this true</param>
        public void UpdateCustomerOrderDetails(Order order, bool isEntriesUpdated = false)
        {
            // Exception handling
            if (order.Id == null)
                throw new ArgumentNullException(nameof(order.Id),
                    "Attempted updating order without an Id");
            if (order.OrderItems == null)
                throw new ArgumentNullException(nameof(order.OrderItems),
                    "Attempted updating order without orderEntries initialized");
            if (order.IsSettled && order.CustomerId == null)
                throw new ArgumentException("Attempted updating creditorder without specifying customerId",
                    nameof(order.CustomerId));
            if (isEntriesUpdated && order.OrderItems == null)
                throw new ArgumentNullException(
                    nameof(order.OrderItems),
                    "Attempted updating order entries with OrderItems = null");

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var customerOrderDal = new OrderDal(connection);
                    customerOrderDal.Update(TODO);

                    if (order.IsSettled)
                        customerOrderDal.InsertAsCreditOrder((uint) order.Id, (uint) order.CustomerId);

                    if (isEntriesUpdated)
                    {
                        customerOrderDal.Delete((uint) order.Id);
                        customerOrderDal.InsertOrderItems((uint) order.Id,
                            order.OrderItems);
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Adds a new Supply Order
        /// </summary>
        /// <param name="supplyorder"></param>
        public void AddSupplyOrder(Purchase supplyorder)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var supplyOrderDal = new PurchaseDal(connection);
                    supplyOrderDal.Insert(supplyorder, User.CurrentUser.EmployeeId);

                    if (supplyorder.Id == null)
                        throw new ArgumentNullException(nameof(supplyorder.Id),
                            "Supply order Id has not been assigned after insert");

                    supplyOrderDal.InsertPurchasedItems((uint) supplyorder.Id, supplyorder.OrderEntries);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Returns list of supplyorders matching the given note text
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public IEnumerable<Purchase> GetSupplyOrders(string note)
        {
            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search(note);
            }
        }

        /// <summary>
        ///     Updates a supply order
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="isEntriesUpdated">If order entries need to be updated, set this true</param>
        public void UpdateSupplyOrder(Purchase purchase, bool isEntriesUpdated = false)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    if (purchase.Id == null)
                        throw new ArgumentNullException(nameof(purchase.Id), "Supply Order Id is null");

                    var supplyOrderDal = new PurchaseDal(connection);
                    supplyOrderDal.Update(purchase);

                    if (!isEntriesUpdated) return;

                    if (purchase.OrderEntries == null)
                        throw new ArgumentNullException(
                            nameof(purchase.OrderEntries),
                            "Attempt to update order entries while OrderItems = null");

                    supplyOrderDal.RemovePurchasedItems((uint) purchase.Id);
                    supplyOrderDal.InsertPurchasedItems((uint) purchase.Id, purchase.OrderEntries);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Returns list of supplyorders from a given supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="isLimited"></param>
        /// <returns></returns>
        public IEnumerable<Purchase> GetSupplyOrders(Supplier supplier, bool isLimited = true)
        {
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier), "Supplier Id is null");

            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search(
                    (uint) supplier.Id,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }

        /// <summary>
        ///     Load Order Entries of a supply order
        /// </summary>
        /// <param name="purchase"></param>
        public void LoadSupplyOrderEntries(Purchase purchase)
        {
            using (var connection = Connector.GetConnection())
            {
                new PurchaseDal(connection).LoadPurchasedItems(purchase);
            }
        }

        /// <summary>
        ///     Gets a list of most recent supply orders.
        ///     The order items are also loaded at this point.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Purchase> GetRecentSupplyOrders(bool isLimited = true)
        {
            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search((uint) (isLimited
                    ? Constraints.DefaultLimit
                    : Constraints.ExtendedLimit));
            }
        }

        /// <summary>
        ///     Returns all due supply orders from passed supplier
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Purchase> GetDueSupplyOrders(Supplier supplier)
        {
            // Exception handling
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id is null");

            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search((uint) supplier.Id, Constraints.DefaultLimit,
                    SupplyOrderStatus.Pending);
            }
        }

        /// <summary>
        ///     Updates a supply order as paid off
        /// </summary>
        /// <param name="purchase"></param>
        public void PayoffSupplyOrder(Purchase purchase)
        {
            // Exception handling
            if (purchase.Id == null)
                throw new ArgumentNullException(
                    nameof(purchase.Id),
                    "Supply order Id is null");

            using (var connection = Connector.GetConnection())
            {
                new PurchaseDal(connection).Update(purchase, SupplyOrderStatus.Paid);
                purchase.Status = SupplyOrderStatus.Paid;
            }
        }

        /// <summary>
        ///     Update a set of supply orders as paid off
        /// </summary>
        /// <param name="supplyOrders"></param>
        public void PayoffSupplyOrders(IEnumerable<Purchase> supplyOrders)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var supplyOrderDal = new PurchaseDal(connection);
                    foreach (var supplyOrder in supplyOrders)
                    {
                        supplyOrder.Status = SupplyOrderStatus.Paid;
                        supplyOrderDal.Update(supplyOrder, SupplyOrderStatus.Paid);
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates a supply order as cancelled
        /// </summary>
        /// <param name="purchase"></param>
        public void CancelSupplyOrder(Purchase purchase)
        {
            // Exception handling
            if (purchase.Id == null)
                throw new ArgumentNullException(
                    nameof(purchase.Id),
                    "Supply order Id is null");

            using (var connection = Connector.GetConnection())
            {
                new PurchaseDal(connection).Update(purchase, SupplyOrderStatus.Cancelled);
                purchase.Status = SupplyOrderStatus.Cancelled;
            }
        }
    }
}