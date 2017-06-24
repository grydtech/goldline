using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class BatteryDal : Dal
    {
        public BatteryDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [batteries] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="Capacity, Voltage"></param>
        /// <param name="capacity"></param>
        /// <param name="voltage"></param>
        internal void Insert(uint productId, string brand, string capacity, string voltage)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into batteries (id_product, brand, capacity, voltage) values (@productId, @brand, @capacity, @voltage)",
                new {productId, brand, capacity, voltage});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [batteries_brands] table
        /// </summary>
        /// <param name="brand"></param>
        internal void InsertBrand(string brand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into batteries_brands values (@brand)",
                new {brand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [batteries] table
        /// </summary>
        /// <param name="nameExp"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Battery> Search(string nameExp = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Capacity, Voltage from batteries " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                (nameExp == null ? "" : "where name_product LIKE @nameExp ") +
                "order by name_product limit @offset, @limit",
                new {nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<Battery>(command);
        }

        /// <summary>
        ///     Searches records in [batteries_brands] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchBrand()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from batteries_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Updates a record in [batteries] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="capacity"></param>
        /// <param name="voltage"></param>
        internal void Update(uint productId, string brand = null, string capacity = null, string voltage = null)
        {
            if (brand == null && capacity == null && voltage == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update batteries set " +
                ((brand == null ? "" : "brand = @brand, ") +
                 (capacity == null ? "" : "capacity = @capacity, ") +
                 (voltage == null ? "" : "voltage = @voltage, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, brand, capacity, voltage});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}