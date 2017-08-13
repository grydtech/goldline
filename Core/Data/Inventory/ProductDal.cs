using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Domain.Enums;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class ProductDal : Dal
    {
        internal ProductDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [products] table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        internal void Insert(string name, ProductType type)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert ignore into products (name_product, type_product) values (@name, @type)",
                new {name, type = type.ToString()});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [products] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="nameExp"></param>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Product> Search(uint? productId = null, string nameExp = null, ProductType? type = null,
            int offset = 0,
            int limit = int.MaxValue)
        {
            switch (type)
            {
                case ProductType.Alloywheel:
                    return new AlloywheelDal(Connection).Search(productId, nameExp, offset, limit);
                case ProductType.Battery:
                    return new BatteryDal(Connection).Search(productId, nameExp, offset, limit);
                case ProductType.Tyre:
                    return new TyreDal(Connection).Search(productId, nameExp, offset, limit);
                case ProductType.Service:
                    return new ServiceDal(Connection).Search(productId, nameExp, offset, limit);
                case null:
                    return new AlloywheelDal(Connection).Search(productId, nameExp, offset, limit).Cast<Product>()
                        .Concat(new BatteryDal(Connection).Search(productId, nameExp, offset, limit))
                        .Concat(new TyreDal(Connection).Search(productId, nameExp, offset, limit))
                        .Concat(new ServiceDal(Connection).Search(productId, nameExp, offset, limit))
                        .OrderBy(c => c.Name);
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Updates a record in [products] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        internal void Update(uint productId, string name = null, ProductType? type = null)
        {
            if (name == null && type == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update products set " +
                ((name == null ? "" : "name_product = @name, ") +
                 (type == null ? "" : "type_product = @type, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, name, type = type?.ToString()});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}