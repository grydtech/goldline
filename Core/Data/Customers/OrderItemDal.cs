using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class OrderItemDal : Dal
    {
        internal OrderItemDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts records into [orders_items] table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderItems"></param>
        internal void InsertMultiple(uint orderId, IEnumerable<OrderItem> orderItems)
        {
            try
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into orders_products (id_order, id_product, unit_price, qty) " +
                    "values (@orderId, @id_product, @unit_price, @qty)",
                    orderItems.Select(o => new
                    {
                        orderId,
                        id_product = o.ProductId,
                        unit_price = o.UnitPrice,
                        qty = o.Qty
                    }));

                // Execute sql command
                Connection.Execute(command);
            }
            catch (ArgumentNullException ex)
            {
                Debug.Write("Argument null excep");
            }
            
            
        }

        /// <summary>
        ///     Searches records in [orders_items] table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal IEnumerable<OrderItem> Search(uint? orderId = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'ProductId', name_product 'ProductName', unit_price 'UnitPrice', qty 'Qty' from orders_products " +
                "join products USING(id_product) " +
                (orderId == null ? "" : "where id_order = @orderId ") +
                "limit @offset, @limit",
                new {orderId, offset, limit});

            // Execute sql command
            return Connection.Query<OrderItem>(command);
        }
    }
}