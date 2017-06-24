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
        private void Insert(string name, ProductType type)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into products (name_product, type_product) values (@name, @type)",
                new {name, type});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [products] table
        /// </summary>
        /// <param name="nameExp"></param>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Product> Search(string nameExp = null, ProductType? type = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', type_product-1 'ProductType' from products " +
                (nameExp == null && type == null ? "" : "where ") +
                (nameExp == null ? "" : "name_product LIKE @nameExp ") +
                (type == null ? "" : (nameExp == null ? "" : "and ") + "type_product = @type ") +
                "order by name_product limit @offset, @limit",
                new {nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<dynamic>(command).Select(o =>
            {
                Product product;

                switch (o.ProductType)
                {
                    case ProductType.Alloywheel:
                        product = new Alloywheel();
                        break;
                    case ProductType.Battery:
                        product = new Battery();
                        break;
                    case ProductType.Tyre:
                        product = new Tyre();
                        break;
                    case ProductType.Service:
                        product = new Service();
                        break;
                    default:
                        return null;
                }

                product.Id = (uint)o.Id;
                product.Name = o.Name;
                return product;
            });
        }

        /// <summary>
        ///     Updates a record in [products] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        private void Update(uint productId, string name = null, ProductType? type = null)
        {
            if (name == null && type == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update products set " +
                ((name == null ? "" : "name_product = @name, ") +
                 (type == null ? "" : "type_product = @type, ")).TrimEnd(' ', ',') +
                " where id_product = @productId",
                new {productId, name, type});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}