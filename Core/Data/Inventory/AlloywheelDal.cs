using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class AlloywheelDal : Dal
    {
        public AlloywheelDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [alloywheels] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="dimension"></param>
        internal void Insert(uint productId, string brand, string dimension)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into alloywheels (id_product, brand, dimension) values (@productId, @brand, @dimension)",
                new {productId, brand, dimension});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [alloywheels_brands] table
        /// </summary>
        /// <param name="brand"></param>
        internal void InsertBrand(string brand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into alloywheels_brands values (@brand)",
                new {brand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [alloywheels_dimensions] table
        /// </summary>
        /// <param name="dimension"></param>
        internal void InsertDimension(string dimension)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into alloywheels_dimensions values (@dimension)",
                new {dimension});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [alloywheels] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="nameExp"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Alloywheel> Search(uint? productId = null, string nameExp = null, int offset = 0,
            int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Model', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension from alloywheels " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                (productId == null ? "" : "where id_product = @productId ") +
                (nameExp == null ? "" : $"{(productId == null ? "where" : "and")} name_product LIKE @nameExp ") +
                "order by name_product limit @offset, @limit",
                new {productId, nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<Alloywheel>(command);
        }

        /// <summary>
        ///     Searches records in [alloywheels_brands] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchBrand()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from alloywheels_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Searches records in [alloywheels_dimensions] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchDimension()
        {
            // Define sql command
            var command = new CommandDefinition("select dimension from alloywheels_dimensions");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Updates a record in [alloywheels] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="dimension"></param>
        internal void Update(uint productId, string brand = null, string dimension = null)
        {
            if (brand == null && dimension == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update alloywheels set " +
                ((brand == null ? "" : "brand = @brand, ") +
                 (dimension == null ? "" : "dimension = @dimension, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, brand, dimension});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}