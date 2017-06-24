using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class OrderDal : Dal
    {
        internal OrderDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [orders] table
        /// </summary>
        /// <param name="customerId">user who inserts the order</param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        internal void Insert(uint? customerId, decimal amount, string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into orders (id_customer, amount, note) values (@customerId, @amount, @note)",
                new { customerId, amount, note });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [orders] table
        /// </summary>
        /// <param name="noteExp"></param>
        /// <param name="offset"></param>
        /// <param name="limit">number of orders returned</param>
        /// <param name="isCredit">if null, return both credit and non credit orders, else return only given type</param>
        /// <param name="customerId"></param>
        internal IEnumerable<Order> Search(bool? isCredit = null, uint? customerId = null, string noteExp = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_order 'Id', amount 'Amount', note 'Note', date_order 'Date', " +
                "due_amount 'DueAmount', is_cancelled 'IsCancelled' from orders_view " +
                (isCredit == null && customerId == null && noteExp == null ? "" : "where ") +
                (isCredit == null ? "" : $"(due_amount > 0) = @isCredit ") +
                (customerId == null ? "" : (isCredit == null ? "" : "and ") + "customerId = @customerId ") +
                (noteExp == null ? "" : (isCredit == null && customerId == null ? "" : "and ") + "note LIKE @noteExp ") +
                "order by id_order desc limit @offset, @limit",
                new {offset, limit, isCredit, customerId, noteExp});

            // Execute sql command
            return Connection.Query<Order>(command);
        }

        /// <summary>
        ///     Updates a record in [orders] table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerId"></param>
        /// <param name="note"></param>
        /// <param name="isCancelled"></param>
        internal void Update(uint id, uint? customerId = null, string note = null, bool? isCancelled = null)
        {
            // Exception Handling
            if (customerId == null && note == null && isCancelled == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update orders set " +
                ((customerId == null ? "" : "id_customer = @customerId, ") +
                (note == null ? "" : "note = @note, ") +
                (isCancelled == null ? "" : "is_cancelled = @isCancelled, ")).TrimEnd(' ', ',') +
                " where id_order = @id",
                new {id, customerId, note, isCancelled});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [orders] table
        /// </summary>
        /// <param name="id"></param>
        internal void Delete(uint id)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from orders where id_order = @id",
                new {id});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}