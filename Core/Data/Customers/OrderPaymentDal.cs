using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class OrderPaymentDal : Dal
    {
        public OrderPaymentDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [orders_payments] table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        internal void Insert(uint orderId, decimal amount)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into orders_payments (id_order, amount) values (@orderId, @amount)",
                new {orderId, amount});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [orders_payments] table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal IEnumerable<OrderPayment> Search(uint? orderId = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', @orderId 'OrderId', date_paid 'Date' amount_paid 'Amount' " +
                "from orders_payments " +
                (orderId == null ? "" : "where id_order = @orderId ") +
                "order by id_payment desc limit @offset, @limit",
                new {orderId, offset, limit});

            // Execute sql command
            return Connection.Query<OrderPayment>(command);
        }

        /// <summary>
        ///     Deletes a record from [orders_payments] table
        /// </summary>
        /// <param name="id"></param>
        internal void Delete(uint id)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from orders_payments where id_payment = @id",
                new {id});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}