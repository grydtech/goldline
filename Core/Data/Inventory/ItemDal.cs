using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Domain.Enums;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class ItemDal : Dal
    {
        public ItemDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [items] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockQty"></param>
        /// <param name="unitPrice"></param>
        internal void Insert(uint productId, uint stockQty, decimal unitPrice)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into items (id_product, qty_stocks, unit_price) values (@productId, @stockQty, @unitPrice)",
                new {productId, stockQty, unitPrice});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [items] table
        /// </summary>
        /// <param name="nameExp"></param>
        /// <param name="productType"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Item> Search(string nameExp = null, ProductType? productType = null, int offset = 0,
            int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', type_product-1 'ProductType', " +
                "qty_stocks 'StockQty', unit_price 'UnitPrice' from items " +
                "join products USING(id_product) " +
                (nameExp == null && productType == null ? "" : "where ") +
                (nameExp == null ? "" : "name_product like @name ") +
                (productType == null ? "" : (nameExp == null ? "" : "and ") + "type_product-1 = @productType ") +
                "order by name_product limit @offset, @limit",
                new {nameExp, productType, offset, limit});

            // Execute sql command
            return Connection.Query<dynamic>(command).Select(o =>
            {
                Item item;

                switch ((ProductType) o.ProductType)
                {
                    case ProductType.Alloywheel:
                        item = new Alloywheel();
                        break;
                    case ProductType.Battery:
                        item = new Battery();
                        break;
                    case ProductType.Tyre:
                        item = new Tyre();
                        break;
                    default:
                        Console.WriteLine("Enum value was invalid when initializing");
                        throw new ArgumentException();
                }

                item.Id = (uint) o.Id;
                item.Name = o.Name;
                item.StockQty = o.StockQty;
                item.UnitPrice = o.UnitPrice;
                return item;
            });
        }

        /// <summary>
        ///     Updates a record in [items] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockQty"></param>
        /// <param name="unitPrice"></param>
        internal void Update(uint productId, uint? stockQty = null, decimal? unitPrice = null)
        {
            if (stockQty == null && unitPrice == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update items set " +
                ((stockQty == null ? "" : "qty_stocks = @stockQty, ") +
                 (unitPrice == null ? "" : "unit_price = @unitPrice, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, stockQty, unitPrice});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}