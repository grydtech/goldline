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
        ///     Inserts a new orderPayment into database
        /// </summary>
        /// <param name="orderPayment"></param>
        internal void InsertOrderPayment(OrderPayment orderPayment)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into orders_payments (id_order, amount) values (@id_order, @amount)",
                new
                {
                    id_customer = orderPayment.OrderId,
                    amount = orderPayment.Amount
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            orderPayment.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Removes given customer payment from database
        /// </summary>
        /// <param name="paymentId"></param>
        internal void RemoveOrderPayment(uint paymentId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from orders_payments where id_payment = @paymentId",
                new
                {
                    paymentId
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns payments of a given order from database
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal IEnumerable<OrderPayment> GetOrderPayments(uint orderId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', @orderId 'OrderId', date_paid 'Date' amount_paid 'Amount' " +
                "from orders_payments " +
                "where id_order = @orderId " +
                "order by id_payment desc",
                new
                {
                    orderId
                });

            // Execute sql command
            return Connection.Query<OrderPayment>(command);
        }

        /// <summary>
        ///     Returns a list of most recent CustomerPayments.
        ///     The number of rows loaded is given as recordLimit.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal IEnumerable<OrderPayment> GetRecentPayments(uint limit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_order 'OrderId', date_paid 'Date', amount_paid 'Amount', note 'Note', id_customer 'CustomerId' " +
                "from orders_payments JOIN orders USING (id_order) " +
                "order by id_payment desc limit @limit",
                new
                {
                    limit
                });

            // Execute sql command
            return Connection.Query<OrderPayment>(command);
        }
    }
}