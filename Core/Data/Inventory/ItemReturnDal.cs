﻿using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class ItemReturnDal : Dal
    {
        internal ItemReturnDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [items_returns] table
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="customerId"></param>
        /// <param name="contactInfo"></param>
        /// <param name="qty"></param>
        /// <param name="isHandled"></param>
        /// <param name="note"></param>
        internal void Insert(uint itemId, uint? customerId, string contactInfo, uint qty, bool isHandled, string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into items_returns (id_item, id_customer, contact_info, qty_return, is_handled, note) " +
                "values (@itemId, @customerId, @contactInfo, @qty, @isHandled, @note)",
                new {itemId, customerId, contactInfo, qty, isHandled, note});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [items_returns] table
        /// </summary>
        /// <param name="noteExp">search by note</param>
        /// <param name="itemId">search by item id</param>
        /// <param name="isHandled">if null, return all irrespective of return condition, else return only given type</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="offset"></param>
        /// <param name="limit">number of items returned</param>
        internal IEnumerable<ItemReturn> Search(string noteExp = null, uint? itemId = null, bool? isHandled = null,
            DateTime? startDate = null, DateTime? endDate = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_return 'Id', date_return 'Date', id_item 'ItemId', id_customer 'CustomerId', name 'CustomerName', " +
                "qty_return 'ReturnQty', is_handled 'IsHandled', contact_info 'ContactInfo', note 'Note' " +
                "from items_returns LEFT JOIN customers USING(id_customer) " +
                (noteExp == null && itemId == null && isHandled == null && startDate == null && endDate == null ? "" : "where ") +
                (noteExp == null ? "" : "note like @noteExp ") +
                (itemId == null ? "" : (noteExp == null ? "" : "and ") + "id_item = @itemId ") +
                (isHandled == null ? "" : (noteExp == null && itemId == null ? "" : "and ") + "is_handled = @isHandled ") +
                (startDate == null
                    ? ""
                    : (noteExp == null && itemId == null && isHandled == null ? "" : "and ") + "date_return >= @startDate ") +
                (endDate == null
                    ? ""
                    : (noteExp == null && itemId == null && isHandled == null && startDate == null ? "" : "and ") +
                      "date_return <= @endDate ") +
                "order by id_return desc limit @offset, @limit",
                new {noteExp, itemId, isHandled, startDate, endDate, offset, limit});

            // Execute sql command
            return Connection.Query<ItemReturn>(command);
        }

        /// <summary>
        ///     Updates a record in [items_returns] table
        /// </summary>
        /// <param name="itemReturnId"></param>
        /// <param name="customerId"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="isHandled"></param>
        /// <param name="note"></param>
        internal void Update(uint itemReturnId, uint? customerId, uint? itemId = null, uint? qty = null,
            bool? isHandled = null,
            string note = null)
        {
            if (itemId == null && qty == null && isHandled == null && note == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update items_returns set " +
                ((customerId == null ? "" : "id_customer = @customerId, ") +
                 (itemId == null ? "" : "id_item = @itemId, ") +
                 (qty == null ? "" : "qty_return = @qty, ") +
                 (isHandled == null ? "" : "is_handled = @isHandled, ") +
                 (note == null ? "" : "note = @note, ")).TrimEnd(' ', ',') +
                " where id_return = @itemReturnId",
                new {itemReturnId, customerId, itemId, qty, isHandled, note});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [items_returns] table
        /// </summary>
        /// <param name="id"></param>
        internal void Delete(uint id)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from items_returns where id_return = @id",
                new {id});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}