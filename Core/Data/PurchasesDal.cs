using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Model.Enums;
using Core.Model.Orders;
using Dapper;

namespace Core.Data
{
    internal class PurchasesDal : Dal
    {
        internal PurchasesDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Inserts a new supplyorder into database and assigns its Id.
        ///     This method does not insert the supplyorder entries.
        /// </summary>
        /// <param name="supplierOrder"></param>
        /// <param name="userId"></param>
        internal void InsertSupplyOrder(SupplierOrder supplierOrder, uint userId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into supplyorders (id_supplier, total, note, status_supplyorder, id_user) " +
                "values (@id_supplier, @total, @note, @status_supplyorder, @id_user)",
                new
                {
                    id_supplier = supplierOrder.SupplierId,
                    total = supplierOrder.Total,
                    note = supplierOrder.Note,
                    status_supplyorder = supplierOrder.Status.ToString(),
                    id_user = userId
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            supplierOrder.Id = GetLastInsertId();
            supplierOrder.UserId = userId;
        }

        /// <summary>
        ///     Inserts the supplyorder entries for a supply order into database
        /// </summary>
        /// <param name="supplyOrderId"></param>
        /// <param name="supplyOrderEntries"></param>
        internal void InsertSupplyOrderEntries(uint supplyOrderId, IEnumerable<SupplierOrderEntry> supplyOrderEntries)
        {
            foreach (var supplyOrderEntry in supplyOrderEntries)
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into supplyorders_entries (id_supplyorder, id_item, qty_item) " +
                    "values (@id_supplyorder, @id_item, @qty_item)",
                    new
                    {
                        id_supplyorder = supplyOrderId,
                        id_item = supplyOrderEntry.ItemId,
                        qty_item = supplyOrderEntry.Qty
                    });

                // Execute sql command
                Connection.Execute(command);
            }
        }

        /// <summary>
        ///     Removes all supplyorder entries from database.
        ///     This method is used to clear the supplyorder entries when updating the supplyorder
        /// </summary>
        /// <param name="supplyOrderId"></param>
        internal void RemoveSupplyOrderEntries(uint supplyOrderId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from supplyorders_entries where id_supplyorder = @id_supplyorder",
                new {id_supplyorder = supplyOrderId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Load Supply order entry details into passed supply order object
        /// </summary>
        /// <param name="supplierOrder"></param>
        internal void LoadSupplyOrderEntries(SupplierOrder supplierOrder)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_item 'ItemId', name_product 'ItemName' qty_item 'Qty' from supplyorders_entries " +
                "join products on supplyorders_entries.id_item = products.id_product " +
                "where id_supplyorder = @id_supplyorder",
                new {id_supplyorder = supplierOrder.Id});

            // Execute sql command
            supplierOrder.OrderEntries = Connection.Query<SupplierOrderEntry>(command).ToList();
        }

        /// <summary>
        ///     Updates an existing supplyorder in database.
        ///     The properties that can be updated are : SupplierId, Total, Note
        ///     This method does not update the supplyorder entries.
        /// </summary>
        /// <param name="supplierOrder"></param>
        internal void UpdateSupplyOrderDetails(SupplierOrder supplierOrder)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update supplyorders set id_supplier = @id_supplier, total = @total, note = @note " +
                "where id_supplier = @id_supplier",
                new
                {
                    id_supplier = supplierOrder.SupplierId,
                    total = supplierOrder.Total,
                    note = supplierOrder.Note
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list most recent supply orders from database
        /// </summary>
        /// <param name="recordLimit">number of supplyorders returned</param>
        /// <returns></returns>
        internal IEnumerable<SupplierOrder> GetRecentSupplyOrders(uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplyorder 'Id', id_supplier 'SupplierId', Total, Note, status_supplyorder-1 'Status', " +
                "date_supplyorder 'Date', id_user 'UserId' from supplyorders " +
                "order by date_supplyorder desc limit @limit",
                new {limit = recordLimit});

            // Execute sql command
            return Connection.Query<SupplierOrder>(command);
        }

        /// <summary>
        ///     Returns a list of supplyorders of a given supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="recordLimit">number of supplyorders returned if all orders are returned</param>
        /// <param name="status">either pending, paid, or cancelled. if this is not null, recordLimit is disregarded</param>
        internal IEnumerable<SupplierOrder> GetSupplyOrders(uint supplierId, uint recordLimit,
            SupplyOrderStatus? status = null)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplyorder 'Id', id_supplier 'SupplierId', Total, Note, status_supplyorder-1 'Status', " +
                "date_supplyorder 'Date', id_user 'UserId' from supplyorders " +
                "where id_supplier = @id_supplier " +
                "order by date_supplyorder desc limit = @limit",
                new
                {
                    id_supplier = supplierId,
                    limit = recordLimit
                });

            var commandStatus = new CommandDefinition(
                "select id_supplyorder 'Id', id_supplier 'SupplierId', Total, Note, status_supplyorder-1 'Status', " +
                "date_supplyorder 'Date', id_user 'UserId' from supplyorders " +
                "where id_supplier = @id_supplier and status_supplyorder = @status_supplyorder " +
                "order by date_supplyorder",
                new
                {
                    id_supplier = supplierId,
                    status_supplyorder = status.ToString()
                });

            // Execute sql command
            return Connection.Query<SupplierOrder>(status == null ? command : commandStatus);
        }

        /// <summary>
        ///     Returns a list of supplyorders matching the given search parameters
        /// </summary>
        /// <param name="note">search by note</param>
        internal IEnumerable<SupplierOrder> GetSupplyOrders(string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplyorder 'Id', id_supplier 'SupplierId', Total, Note, status_supplyorder-1 'Status', " +
                "date_supplyorder 'Date', id_user 'UserId' from supplyorders " +
                "where note = @note_supplyorder " +
                "order by date_supplyorder desc",
                new
                {
                    note_supplyorder = note
                });

            // Execute sql command
            return Connection.Query<SupplierOrder>(command);
        }

        /// <summary>
        ///     Returns a list of all due supplier invoices, with earliest invoices first
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SupplierOrder> GetAllDueSupplyOrders()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplyorder 'Id', id_supplier 'SupplierId', Total, Note, status_supplyorder-1 'Status', " +
                "date_supplyorder 'Date', id_user 'UserId' from supplyorders " +
                "where status_supplyorder = 'Pending' " +
                "order by date_supplyorder");

            // Execute sql command
            return Connection.Query<SupplierOrder>(command);
        }

        /// <summary>
        ///     Updates existing supply order status
        /// </summary>
        /// <param name="supplierOrder"></param>
        /// <param name="supplyOrderStatus">Either Pending, Paid or Cancelled</param>
        internal void UpdateSupplyOrderStatus(SupplierOrder supplierOrder, SupplyOrderStatus supplyOrderStatus)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update supplyorders set status_supplyorder = @status_supplyorder where id_supplier = @id_supplier",
                new
                {
                    id_supplier = supplierOrder.SupplierId,
                    status_supplyorder = supplierOrder.Status
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}