using System;
using System.Collections.Generic;
using System.Data;
using Core.Model;
using Core.Model.Enums;
using Dapper;

namespace Core.Data
{
    internal class ItemsReturnsDal : Dal
    {
        internal ItemsReturnsDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Inserts a new item return to database and assigns its Id
        /// </summary>
        /// <param name="itemReturn"></param>
        internal void InsertItemReturn(ItemReturn itemReturn)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into items_returns (id_item, id_customer, qty, is_handled, note) " +
                "values (@id_item, @id_customer, @qty, @is_handled, @note)",
                new
                {
                    id_item = itemReturn.ItemId,
                    id_customer = itemReturn.CustomerId,
                    qty = itemReturn.ReturnQty,
                    is_handled = itemReturn.Condition == ReturnCondition.Completed,
                    note = itemReturn.Note
                }, Transaction);

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            itemReturn.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Updates the details of an item return
        ///     The properties that can be updated are : ItemId, ReturnQty, Note
        /// </summary>
        /// <param name="id_return"></param>
        /// <param name="id_item"></param>
        /// <param name="qty"></param>
        /// <param name="is_handled"></param>
        /// <param name="note"></param>
        internal void UpdateItemReturn(uint id_return, uint? id_item = null, uint? qty = null, bool? is_handled = null, string note = null )
        {
            // Define sql command
            var command = new CommandDefinition(
                SqlGenerator.Update("items_returns").Set(new {id_item, qty, note, is_handled}).Where(new {id_return}),
                null,
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Removes an existing item return from database
        /// </summary>
        /// <param name="itemReturnId"></param>
        internal void DeleteItemReturn(uint itemReturnId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from items_returns where id_return = @id_return",
                new {id_return = itemReturnId},
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of item returns matching given search parameters
        /// </summary>
        /// <param name="note">search by note</param>
        /// <param name="is_handled">if null, return all irrespective of return condition, else return only given type</param>
        /// <param name="limit">number of items returned</param>
        internal IEnumerable<ItemReturn> GetItemReturns(string note, bool is_handled, uint limit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_return 'Id', id_item 'ItemId', id_customer 'CustomerId', qty 'ReturnQty', " +
                "date_return 'Date', is_handled 'IsHandled', note 'Note' " +
                "from items_returns where note like @note and is_handled = @is_handled " +
                "order by id_return desc limit @limit",
                new
                {
                    note,
                    limit,
                    is_handled,
                }, Transaction);

            // Execute sql command
            return Connection.Query<ItemReturn>(command);
        }

        /// <summary>
        ///     Returns a list of all item returns betweeen the given dates
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        internal IEnumerable<ItemReturn> GetAllItemReturns(DateTime start_date, DateTime end_date)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_return 'Id', id_item 'ItemId', id_customer 'CustomerId', qty 'ReturnQty', " +
                "date_return 'Date', is_handled 'IsHandled', note 'Note' " +
                "from items_returns where date_return between @start_date and @end_date",
                new
                {
                    start_date,
                    end_date
                }, Transaction);

            // Execute sql command
            return Connection.Query<ItemReturn>(command);
        }
    }
}