using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Model.Orders;
using Dapper;

namespace Core.Data
{
    internal class OrderDal : Dal
    {
        internal OrderDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a new customer order into database and assign its Id
        ///     Order entries should be inserted seperately
        /// </summary>
        /// <param name="customerOrder"></param>
        /// <param name="userId">user who inserts the order</param>
        internal void InsertOrder(CustomerOrder customerOrder, uint customerId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into orders (id_customer, amount, note) values (@id_customer, @amount, @note)",
                new
                {
                    id_customer = customerId,
                    amount = customerOrder.Total,
                    note = customerOrder.Note
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customerOrder.Id = GetLastInsertId();
            customerOrder.CustomerId = customerId;
        }

        /// <summary>
        ///     Inserts new order entries into database
        ///     This method should be run after InsertOrder()
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderEntries"></param>
        internal void InsertOrderEntries(uint orderId, IEnumerable<OrderEntry> orderEntries)
        {
            foreach (var orderEntry in orderEntries)
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into orders_products (id_order, id_product, unit_price, qty) " +
                    "values (@id_order, @id_product, @unit_price, @qty)",
                    new
                    {
                        id_order = orderId,
                        id_product = orderEntry.ProductId,
                        unit_price = orderEntry.UnitPrice,
                        qty = orderEntry.Qty
                    });

                // Execute sql command
                Connection.Execute(command);
            }
        }

        /// <summary>
        ///     Remove order from database
        /// </summary>
        /// <param name="orderId"></param>
        internal void RemoveOrder(uint orderId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from orders where id_order = @id_order",
                new {id_order = orderId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of recent customer orders.
        ///     Note: You should call LoadOrderEntries() seperately to load orderEntries.
        /// </summary>
        /// <param name="limit">number of orders returned</param>
        /// <param name="isCredit">if null, return both credit and non credit orders, else return only given type</param>
        internal IEnumerable<CustomerOrder> GetRecentCustomerOrders(uint limit, bool? isCredit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_order 'Id', amount 'Amount', note 'Note', date_order 'Date', order_due_amount(id_order) 'DueAmount', is_cancelled 'IsCancelled' from orders " +
                (isCredit == null ? "" : "where (order_due_amount(id_order) != 0) = @isCredit ") + 
                "order by id_order desc limit @limit",
                new
                {
                    limit, isCredit
                });

            // Execute sql command
            return Connection.Query<CustomerOrder>(command);
        }

        /// <summary>
        ///     Loads the list of order entries into a given order
        /// </summary>
        /// <param name="customerOrder"></param>
        /// <returns></returns>
        internal void LoadOrderEntries(CustomerOrder customerOrder)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'ProductId', name_product 'ProductName', unit_price 'UnitPrice', qty 'Qty' from orders_products " +
                "join products USING(id_product) where id_order = @id_order",
                new
                {
                    id_order = customerOrder.Id
                });

            // Execute sql command
            var orderEntries = Connection.Query<OrderEntry>(command);
            customerOrder.OrderEntries = orderEntries.ToList();
        }

        /// <summary>
        ///     Returns a list of CustomerOrders of a given customer. recordLimit is the number of orders returned.
        ///     Note: You should call LoadOrderEntries() seperately to load orderEntries.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="limit"></param>
        internal IEnumerable<CustomerOrder> GetOrders(uint customerId, uint limit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_order 'Id', date_order 'Date', id_customer = @id_customer, amount 'Amount', note 'Note', (is_settled = false) 'IsCredit', " +
                "is_cancelled 'IsCancelled' from orders " +
                "where id_customer = @id_customer " +
                "order by id_order desc limit @limit",
                new
                {
                    id_customer = customerId,
                    limit
                });

            // Execute sql command
            return Connection.Query<CustomerOrder>(command);
        }

        /// <summary>
        ///     Gets all orders with their note containing given text
        ///     Note: You should call LoadOrderEntries() seperately to load orderEntries.
        /// </summary>
        /// <param name="note">serch by note</param>
        /// <returns></returns>
        internal IEnumerable<CustomerOrder> GetOrders(string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_order 'Id', amount 'Amount', note 'Note', date_order 'Date', is_credit 'IsCredit', is_cancelled 'IsCancelled' from orders " +
                "where note like @note " +
                "order by id_order desc",
                new
                {
                    note = "%" + note + "%"
                });

            // Execute sql command
            return Connection.Query<CustomerOrder>(command);
        }

        /// <summary>
        ///     Updates a customer order in the database.
        ///     The properties that will be update are: Note, IsCredit, IsCancelled
        /// </summary>
        /// <param name="customerOrder"></param>
        internal void UpdateCustomerOrderDetails(CustomerOrder customerOrder)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update orders set note = @note, is_settled = @is_settled, is_cancelled = @is_cancelled where id_order = @id_order",
                new
                {
                    id_order = customerOrder.Id,
                    note = customerOrder.Note,
                    // KEEP IN MIND OF THE 'NOT' CONDITION APPLIED HERE
                    is_credit = !customerOrder.IsCredit,
                    is_cancelled = customerOrder.IsCancelled
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}