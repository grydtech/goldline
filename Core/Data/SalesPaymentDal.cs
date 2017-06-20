using System.Collections.Generic;
using System.Data;
using Core.Model.Payments;
using Dapper;

namespace Core.Data
{
    internal class SalesPaymentDal : Dal
    {
        public SalesPaymentDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Inserts a new customerPayment into database
        /// </summary>
        /// <param name="customerPayment"></param>
        /// <param name="userId">user who inserts the payment</param>
        internal void InsertCustomerPayment(CustomerPayment customerPayment, uint userId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers_payments (id_customer, amount, note, id_user) values (@id_customer, @amount, @note, @id_user)",
                new
                {
                    id_customer = customerPayment.CustomerId,
                    amount = customerPayment.Amount,
                    note = customerPayment.Note,
                    id_user = userId
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customerPayment.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Removes given customer payment from database
        /// </summary>
        /// <param name="customerPaymentId"></param>
        internal void RemoveCustomerPayment(uint customerPaymentId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from customers_payments where id_payment = @id_payment",
                new
                {
                    id_payment = customerPaymentId
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns payments of a given Customer from database
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="recordLimit">number of customer payments returned</param>
        /// <returns></returns>
        internal IEnumerable<CustomerPayment> GetCustomerPayments(uint customerId, uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_customer 'CustomerId', amount 'Amount', note 'Note', date_payment 'Date', id_user 'UserId' " +
                "from customers_payments " +
                "where id_customer = @id_customer " +
                "order by id_payment desc limit @limit",
                new
                {
                    id_customer = customerId,
                    limit = recordLimit
                });

            // Execute sql command
            return Connection.Query<CustomerPayment>(command);
        }

        /// <summary>
        ///     Returns a list of most recent CustomerPayments.
        ///     The number of rows loaded is given as recordLimit.
        /// </summary>
        /// <param name="recordLimit"></param>
        /// <returns></returns>
        internal IEnumerable<CustomerPayment> GetRecentPayments(uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_customer 'CustomerId', amount 'Amount', note 'Note', date_payment 'Date', id_user 'UserId' " +
                "from customers_payments " +
                "order by id_payment desc limit @limit",
                new
                {
                    limit = recordLimit
                });

            // Execute sql command
            return Connection.Query<CustomerPayment>(command);
        }
    }
}