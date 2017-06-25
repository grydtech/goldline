using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class TyreDal : Dal
    {
        public TyreDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [tyres] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="dimension"></param>
        /// <param name="country"></param>
        internal void Insert(uint productId, string brand, string dimension, string country)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into tyres (id_product, brand, dimension, country) values (@productId, @brand, @dimension, @country)",
                new {productId, brand, dimension, country});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [tyres_brands] table
        /// </summary>
        /// <param name="brand"></param>
        internal void InsertBrand(string brand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into tyres_brands values (@brand)",
                new {brand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [tyres_dimensions] table
        /// </summary>
        /// <param name="dimension"></param>
        internal void InsertDimension(string dimension)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into tyres_dimensions values (@dimension)",
                new {dimension});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a record into [countries] table
        /// </summary>
        /// <param name="country"></param>
        internal void InsertCountry(string country)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into countries values (@country)",
                new {country});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [tyres] table
        /// </summary>
        /// <param name="nameExp"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Tyre> Search(string nameExp = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Model', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension, Country from tyres " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                (nameExp == null ? "" : "where name_product LIKE @nameExp ") +
                "order by name_product limit @offset, @limit",
                new {nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<Tyre>(command);
        }

        /// <summary>
        ///     Searches records in [tyres_brands] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchBrand()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from tyres_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Searches records in [tyres_dimensions] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchDimension()
        {
            // Define sql command
            var command = new CommandDefinition("select dimension from tyres_dimensions");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Searcges records in [countries] table
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> SearchCountry()
        {
            // Define sql command
            var command = new CommandDefinition("select country from countries");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Updates a record in [tyres] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="brand"></param>
        /// <param name="dimension"></param>
        /// <param name="country"></param>
        internal void Update(uint productId, string brand = null, string dimension = null, string country = null)
        {
            if (brand == null && dimension == null && country == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update tyres set " +
                ((brand == null ? "" : "brand = @brand, ") +
                 (dimension == null ? "" : "dimension = @dimension, ") +
                 (country == null ? "" : "country = @country, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, brand, dimension, country});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}