using System;
using System.Collections.Generic;
using System.Data;
using Core.Model;
using Core.Model.Enums;
using Dapper;

namespace Core.Data
{
    internal class ItemReturnDal : Dal
    {
        internal ItemReturnDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a new item return to database and assigns its Id
        /// </summary>
        /// <param name="itemReturn"></param>
        /// <param name="userId">user who inserts the item return</param>
        internal void InsertItemReturn(ItemReturn itemReturn, uint userId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into items_returns (id_item, id_customer, qty_return, is_handled, note) " +
                "values (@id_item, @id_customer, @qty_return, @is_handled, @note)",
                new
                {
                    id_item = itemReturn.ItemId,
                    id_customer = itemReturn.CustomerId,
                    qty_return = itemReturn.ReturnQty,
                    is_handled = itemReturn.Condition,
                    note = itemReturn.Note
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            itemReturn.Id = GetLastInsertId();
            itemReturn.UserId = userId;
        }

        /// <summary>
        ///     Updates the details of an item return
        ///     The properties that can be updated are : ItemId, ReturnQty, Note
        /// </summary>
        /// <param name="itemReturn"></param>
        /// <param name="isHandled"></param>
        internal void UpdateItemReturnDetails(ItemReturn itemReturn, bool isHandled)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update items_returns set id_item = @id_item, qty_return = @qty_return, note = @note, is_handled = @is_handled " +
                "where id_return = @id_return",
                new
                {
                    id_return = itemReturn.Id,
                    id_item = itemReturn.ItemId,
                    qty_return = itemReturn.ReturnQty,
                    note = itemReturn.Note
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Removes an existing item return from database
        /// </summary>
        /// <param name="itemReturnId"></param>
        internal void RemoveItemReturn(uint itemReturnId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from items_returns where id_return = @id_return",
                new {id_return = itemReturnId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of item returns matching given search parameters
        /// </summary>
        /// <param name="note">search by note</param>
        /// <param name="isHandled">if null, return all irrespective of return condition, else return only given type</param>
        /// <param name="recordLimit">number of items returned</param>
        internal IEnumerable<ItemReturn> GetItemReturns(string note, bool? isHandled, uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_return 'Id', id_item 'ItemId', id_customer 'OrderId', qty_return 'ReturnQty', " +
                "date_return 'Date', condition_return-1 'ReturnCondition', note 'Note', id_user 'UserId' " +
                "from items_returns where note like @return_note " +
                (isHandled == null ? "" : "and is_handled = @is_handled") +
                "order by id_return desc limit @limit",
                new
                {
                    return_note = note,
                    is_handled = isHandled,
                    limit = recordLimit
                });

            // Execute sql command
            return Connection.Query<ItemReturn>(command);
        }

        /// <summary>
        ///     Returns a list of all item returns betweeen the given dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        internal IEnumerable<ItemReturn> GetAllItemReturns(DateTime startDate, DateTime endDate)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_return 'Id', id_item 'ItemId', id_customer 'OrderId', qty_return 'ReturnQty', " +
                "date_return 'Date', is_handled 'IsHandled', note 'Note' " +
                "from items_returns where date_return between @start_date and @end_date",
                new
                {
                    start_date = startDate,
                    end_date = endDate
                });

            // Execute sql command
            return Connection.Query<ItemReturn>(command);
        }
    }
}