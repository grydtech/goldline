using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Suppliers;
using Dapper;

namespace Core.Data.Suppliers
{
    internal class PurchaseDal : Dal
    {
        internal PurchaseDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [purchases] table
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        /// <param name="isSettled"></param>
        internal void Insert(uint supplierId, decimal amount, string note, bool isSettled)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into purchases (id_supplier, amount, note, is_settled) " +
                "values (@supplierId, @amount, @note, @isSettled)",
                new {supplierId, amount, note, isSettled});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates a record in [purchases] table
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="supplierId"></param>
        /// <param name="amount"></param>
        /// <param name="isSettled"></param>
        /// <param name="note"></param>
        internal void Update(uint purchaseId, uint? supplierId = null, decimal? amount = null, bool? isSettled = null,
            string note = null)
        {
            if (supplierId == null && amount == null && isSettled == null && note == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update purchases set " +
                ((supplierId == null ? "" : "id_supplier = @supplierId, ") +
                 (amount == null ? "" : "amount = @amount, ") +
                 (isSettled == null ? "" : "is_settled = @isSettled, ") +
                 (note == null ? "" : "note = @note, ")).TrimEnd(' ', ',') +
                " where id_purchase = @purchaseId",
                new {purchaseId, supplierId, amount, isSettled, note});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [purchases] table
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="note"></param>
        /// <param name="offset">number of purchases returned if all orders are returned</param>
        /// <param name="isSettled">Null if filter not applied, else true or false, recordLimit is disregarded</param>
        /// <param name="limit"></param>
        internal IEnumerable<Purchase> Search(uint? supplierId = null,
            bool? isSettled = null, string note = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_purchase 'Id', id_supplier 'SupplierId', Amount, Note, is_settled 'IsSettled', " +
                "date_purchased 'Date' from purchases " +
                (supplierId == null && isSettled == null && note == null ? "" : "where ") +
                (supplierId == null ? "" : "id_supplier = @supplierId ") +
                (isSettled == null ? "" : (supplierId == null ? "" : "and ") + "is_settled = @isSettled ") +
                (note == null ? "" : (supplierId == null && isSettled == null ? "" : "and ") + "note = @note ") +
                "order by date_purchased desc limit = @offset, @limit",
                new {supplierId, isSettled, note, offset, limit});

            // Execute sql command
            return Connection.Query<Purchase>(command);
        }

        /// <summary>
        ///     Deletes a records from [purchases] table
        /// </summary>
        /// <param name="purchaseId"></param>
        public void Delete(uint purchaseId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from purchases where id_purchase = @purchaseId",
                new {purchaseId});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}