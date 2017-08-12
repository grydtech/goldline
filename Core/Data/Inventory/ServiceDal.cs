using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Enums;
using Core.Domain.Model.Inventory;
using Dapper;

namespace Core.Data.Inventory
{
    internal class ServiceDal : Dal
    {
        public ServiceDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a service into [products] table
        /// </summary>
        /// <param name="name"></param>
        internal void Insert(string name)
        {
            new ProductDal(Connection).Insert(name, ProductType.Service);
        }

        /// <summary>
        ///     Searches services in [products] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="nameExp"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Service> Search(uint? productId = null, string nameExp = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', type_product-1 'ProductType' from products where " +
                (productId == null ? "" : "id_product = @productId ") +
                (nameExp == null ? "" : $"{(productId == null ? "" : "and")} name_product LIKE @nameExp ") +
                $"{(productId == null && nameExp == null ? "" : "and")} type_product-1 = @type " +
                "order by name_product limit @offset, @limit",
                new {productId, nameExp, offset, limit, type = ProductType.Service});

            // Execute sql command
            return Connection.Query<Service>(command);
        }

        /// <summary>
        ///     Updates a service in [products] table
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="name"></param>
        internal void Update(uint productId, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update products set name_product = @name " +
                "where id_product = @productId and type_product-1 = @type",
                new {productId, name, type = ProductType.Service});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}