using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Model.Orders;
using Dapper;

namespace Core.Data
{
    internal class CustomerOrderDal : Dal
    {
        internal CustomerOrderDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a new customer order into database and assign its Id
        ///     Order entries should be inserted seperately
        /// </summary>
        /// <param name="customerOrder"></param>
        /// <param name="userId">user who inserts the order</param>
        internal void InsertCustomerOrder(CustomerOrder customerOrder, uint userId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into orders (total, note, id_user) values (@total, @note, @id_user)",
                new
                {
                    total = customerOrder.Total,
                    note = customerOrder.Note,
                    id_user = userId
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customerOrder.Id = GetLastInsertId();
            customerOrder.UserId = userId;
        }

        /// <summary>
        ///     Inserts a new credit order into database or ignore if record exists
        ///     Then this method should be run after InsertCustomerOrder() for credit orders
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="customerId"></param>
        internal void InsertAsCreditOrder(uint orderId, uint customerId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert ignore into orders_credit (id_order, id_customer) values (@id_order, @id_customer)",
                new
                {
                    id_order = orderId,
                    id_customer = customerId
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts new order entries into database
        ///     This method should be run after InsertCustomerOrder()
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderEntries"></param>
        internal void InsertCustomerOrderEntries(uint orderId, IEnumerable<CustomerOrderEntry> orderEntries)
        {
            foreach (var customerOrderEntry in orderEntries)
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into orders_entries (id_order, id_product, unit_price, qty_product) " +
                    "values (@id_order, @id_product, @unit_price, @qty_product)",
                    new
                    {
                        id_order = orderId,
                        id_product = customerOrderEntry.ProductId,
                        unit_price = customerOrderEntry.UnitSalePrice,
                        qty_product = customerOrderEntry.Qty
                    });

                // Execute sql command
                Connection.Execute(command);
            }
        }

        /// <summary>
        ///     Removes all existing orderEntries of the customer order.
        ///     This method is run when order entries need to be updated.
        ///     First the existing records are cleared and then the new records are inserted
        /// </summary>
        /// <param name="orderId"></param>
        internal void RemoveCustomerOrderEntries(uint orderId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from orders_entries where id_order = @id_order",
                new {id_order = orderId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of recent customer orders.
        ///     Note: You should call LoadOrderEntries() seperately to load orderEntries.
        /// </summary>
        /// <param name="recordLimit">number of orders returned</param>
        /// <param name="isCredit">if null, return both credit and non credit orders, else return only given type</param>
        internal IEnumerable<CustomerOrder> GetRecentCustomerOrders(uint recordLimit, bool? isCredit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select orders.id_order 'Id', total 'Total', note 'Note', date_order 'Date', is_credit 'IsCredit', " +
                "id_user 'UserId', is_cancelled 'IsCancelled' from orders " +
                "order by orders.id_order desc limit @limit",
                new
                {
                    limit = recordLimit
                });

            var commandActive = new CommandDefinition(
                "select orders.id_order 'Id', total 'Total', note 'Note', date_order 'Date', is_credit 'IsCredit', " +
                "id_user 'UserId', is_cancelled 'IsCancelled' from orders " +
                "where is_credit = @is_credit " +
                "order by orders.id_order desc limit @limit",
                new
                {
                    limit = recordLimit,
                    is_credit = isCredit
                });

            // Execute sql command
            return Connection.Query<CustomerOrder>(isCredit == null ? command : commandActive);
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
                "select orders_entries.id_product 'ProductId', name_product 'ProductName', unit_price 'UnitSalePrice', qty_product 'Qty' from orders_entries " +
                "join products on orders_entries.id_product = products.id_product " +
                "where id_order = @id_order",
                new
                {
                    id_order = customerOrder.Id
                });

            // Execute sql command
            var orderEntries = Connection.Query<CustomerOrderEntry>(command);
            customerOrder.OrderEntries = orderEntries.ToList();
        }

        /// <summary>
        ///     Returns a list of CustomerOrders of a given customer. recordLimit is the number of orders returned.
        ///     Note: You should call LoadOrderEntries() seperately to load orderEntries.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="recordLimit"></param>
        internal IEnumerable<CustomerOrder> GetCustomerOrders(uint customerId, uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select orders.id_order 'Id', total 'Total', note 'Note', date_order 'Date', is_credit 'IsCredit', " +
                "id_user 'UserId', is_cancelled 'IsCancelled' from orders " +
                "where id_customer = @id_customer " +
                "order by orders.id_order desc limit @limit",
                new
                {
                    id_customer = customerId,
                    limit = recordLimit
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
        internal IEnumerable<CustomerOrder> GetCustomerOrders(string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select orders.id_order 'Id', total 'Total', note 'Note', date_order 'Date', is_credit 'IsCredit', " +
                "id_user 'UserId', is_cancelled 'IsCancelled' from orders " +
                "where note like @note " +
                "order by orders.id_order desc",
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
                "update orders set note = @note, is_credit = @is_credit, is_cancelled = @is_cancelled where id_order = @id_order",
                new
                {
                    id_order = customerOrder.Id,
                    note = customerOrder.Note,
                    is_credit = customerOrder.IsCredit,
                    is_cancelled = customerOrder.IsCancelled
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}