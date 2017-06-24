using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Domain.Model.Suppliers;
using Dapper;

namespace Core.Data.Suppliers
{
    internal class PurchaseItemDal : Dal
    {
        public PurchaseItemDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts records into [purchases_items] table
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="purchaseItems"></param>
        internal void InsertMultiple(uint purchaseId, IEnumerable<PurchaseItem> purchaseItems)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into purchases_items (id_purchase, id_item, qty) " +
                "values (@purchaseId, @itemId, @qty)",
                new[] {purchaseItems.Select(i => new {purchaseId, itemId = i.ItemId, qty = i.Qty})});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [purchases_items] table
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<PurchaseItem> Search(uint? purchaseId = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_item 'ItemId', name_product 'ItemName' qty 'Qty' from purchases_items " +
                "join products USING(id_product) " +
                (purchaseId == null ? "" : "where id_purchase = @purchaseId ") +
                "order by id_purchase desc limit @offset, @limit",
                new {purchaseId, offset, limit});

            // Execute sql command
            return Connection.Query<PurchaseItem>(command);
        }

        /// <summary>
        ///     Delete records from [purchases_items] table
        /// </summary>
        /// <param name="purchaseId"></param>
        internal void Delete(uint purchaseId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from purchases_items where id_purchase = @purchaseId",
                new {purchaseId});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}