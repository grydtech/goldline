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
        /// <param name="productId"></param>
        /// <param name="nameExp"></param>
        /// <param name="productType"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Item> Search(uint? productId = null, string nameExp = null,
            ProductType? productType = null, int offset = 0,
            int limit = int.MaxValue)
        {
            switch (productType)
            {
                case ProductType.Alloywheel:
                    return new AlloywheelDal(Connection).Search(productId, nameExp, offset, limit);
                case ProductType.Battery:
                    return new BatteryDal(Connection).Search(productId, nameExp, offset, limit);
                case ProductType.Tyre:
                    return new TyreDal(Connection).Search(productId, nameExp, offset, limit);
                case null:
                    return new AlloywheelDal(Connection).Search(productId, nameExp, offset, limit).Cast<Item>()
                        .Concat(new BatteryDal(Connection).Search(productId, nameExp, offset, limit))
                        .Concat(new TyreDal(Connection).Search(productId, nameExp, offset, limit))
                        .OrderBy(c => c.Name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(productType), productType, null);
            }
        }

        /// <summary>
        ///     Updates a record in [items] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockQty"></param>
        /// <param name="unitPrice"></param>
        /// <param name="stockIncrement"></param>
        internal void Update(uint productId, uint? stockQty = null, decimal? unitPrice = null,
            int? stockIncrement = null)
        {
            if (stockQty == null && unitPrice == null && stockIncrement == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update items set " +
                ((stockQty == null ? "" : "qty_stocks = @stockQty, ") +
                 (stockIncrement == null ? "" : "qty_stocks = qty_stocks+@stockIncrement, ") +
                 (unitPrice == null ? "" : "unit_price = @unitPrice, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, stockQty, unitPrice, stockIncrement});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}