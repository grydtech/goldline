using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Domain.Model.Suppliers;
using Dapper;

namespace Core.Data.Suppliers
{
    internal class PurchaseDal : Dal
    {
        internal PurchaseDal(IDbConnection connection) : base(connection)
        {
        }

        #region Purchases

        /// <summary>
        ///     Inserts a new supplyorder into database and assigns its Id.
        ///     This method does not insert the supplyorder entries.
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="userId"></param>
        internal void InsertPurchase(Purchase purchase, uint userId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into purchases (id_supplier, amount, note, is_settled) " +
                "values (@id_supplier, @amount, @note, @is_settled)",
                new
                {
                    id_supplier = purchase.SupplierId,
                    amount = purchase.Amount,
                    note = purchase.Note,
                    is_settled = purchase.Status.ToString()
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            purchase.Id = GetLastInsertId();
            purchase.UserId = userId;
        }

        /// <summary>
        ///     Updates an existing supplyorder in database.
        ///     The properties that can be updated are : SupplierId, Amount, Note
        ///     This method does not update the supplyorder entries.
        /// </summary>
        /// <param name="purchase"></param>
        internal void UpdatePurchase(Purchase purchase)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update purchases set id_supplier = @id_supplier, amount = @amount, note = @note " +
                "where id_supplier = @id_supplier",
                new
                {
                    id_supplier = purchase.SupplierId,
                    amount = purchase.Amount,
                    note = purchase.Note
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list most recent supply orders from database
        /// </summary>
        /// <param name="recordLimit">number of purchases returned</param>
        /// <returns></returns>
        internal IEnumerable<Purchase> GetRecentPurchases(uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_purchase 'Id', id_supplier 'SupplierId', Amount, Note, is_settled 'IsSettled', " +
                "date_purchased 'Date' from purchases " +
                "order by date_purchased desc limit @limit",
                new {limit = recordLimit});

            // Execute sql command
            return Connection.Query<Purchase>(command);
        }

        /// <summary>
        ///     Returns a list of purchases of a given supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="recordLimit">number of purchases returned if all orders are returned</param>
        /// <param name="isSettled">Null if filter not applied, else true or false, recordLimit is disregarded</param>
        internal IEnumerable<Purchase> GetPurchases(uint supplierId, uint recordLimit,
            bool? isSettled = null)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_purchase 'Id', id_supplier 'SupplierId', Amount, Note, is_settled 'IsSettled', " +
                "date_purchased 'Date' from purchases " +
                "where id_supplier = @id_supplier " +
                (isSettled == null ? "" : "and is_settled = @is_settled ") +
                "order by date_purchased desc limit = @limit",
                new
                {
                    id_supplier = supplierId,
                    limit = recordLimit,
                    is_settled = isSettled.ToString()
                });

            // Execute sql command
            return Connection.Query<Purchase>(command);
        }

        /// <summary>
        ///     Returns a list of purchases matching the given search parameters
        /// </summary>
        /// <param name="note">search by note</param>
        internal IEnumerable<Purchase> GetPurchases(string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_purchase 'Id', id_supplier 'SupplierId', Amount, Note, is_settled 'IsSettled', " +
                "date_purchased 'Date' from purchases " +
                "where note = @note " +
                "order by date_purchased desc",
                new
                {
                    note
                });

            // Execute sql command
            return Connection.Query<Purchase>(command);
        }

        /// <summary>
        ///     Returns a list of all due supplier invoices, with earliest invoices first
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Purchase> GetUnsettledPurchases()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_purchase 'Id', id_supplier 'SupplierId', Amount, Note, is_settled 'Status', " +
                "date_purchased 'Date' from purchases " +
                "where is_settled = false " +
                "order by date_purchased");

            // Execute sql command
            return Connection.Query<Purchase>(command);
        }

        /// <summary>
        ///     Updates existing supply order isSettled
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="isSettled">Either True or False</param>
        internal void UpdatePurchaseSettled(Purchase purchase, bool isSettled)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update purchases set is_settled = @is_settled where id_supplier = @id_supplier",
                new
                {
                    id_supplier = purchase.SupplierId,
                    is_settled = isSettled
                });

            // Execute sql command
            Connection.Execute(command);
        }

        #endregion

        #region Purchased Items

        /// <summary>
        ///     Inserts the supplyorder entries for a supply order into database
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchasedItems"></param>
        internal void InsertPurchasedItems(uint purchaseId, IEnumerable<PurchaseItem> purchasedItems)
        {
            foreach (var item in purchasedItems)
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into purchases_items (id_purchase, id_item, qty) " +
                    "values (@id_purchase, @id_item, @qty)",
                    new
                    {
                        id_purchase = purchaseId,
                        id_item = item.ItemId,
                        qty = item.Qty
                    });

                // Execute sql command
                Connection.Execute(command);
            }
        }

        /// <summary>
        ///     Removes all supplyorder entries from database.
        ///     This method is used to clear the supplyorder entries when updating the supplyorder
        /// </summary>
        /// <param name="purchaseId"></param>
        internal void RemovePurchasedItems(uint purchaseId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from purchases_items where id_purchase = @id_purchase",
                new {id_purchase = purchaseId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Load Supply order entry details into passed supply order object
        /// </summary>
        /// <param name="purchase"></param>
        internal void LoadPurchasedItems(Purchase purchase)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_item 'ItemId', name_product 'ItemName' qty 'Qty' from purchases_items " +
                "join products USING(id_product) " +
                "where id_purchase = @id_purchase",
                new {id_purchase = purchase.Id});

            // Execute sql command
            purchase.OrderEntries = Connection.Query<PurchaseItem>(command).ToList();
        }

        #endregion
    }
}