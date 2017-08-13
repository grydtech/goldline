using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Suppliers;
using Dapper;

namespace Core.Data.Suppliers
{
    internal class SupplierDal : Dal
    {
        internal SupplierDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [suppliers] table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        internal void Insert(string name, string contact)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert ignore into suppliers (name, contact) values (@name, @contact)",
                new {name, contact});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [suppliers] table
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="nameExp">search by name</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Supplier> Search(uint? supplierId = null, string nameExp = null, int offset = 0,
            int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplier 'Id', name 'Name', Contact from suppliers " +
                (supplierId == null ? "" : "where id_supplier = @supplierId ") +
                (nameExp == null ? "" : $"{(supplierId == null ? "where" : "and")} name like @nameExp ") +
                "order by name limit @offset, @limit",
                new {supplierId, nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<Supplier>(command);
        }

        /// <summary>
        ///     Updates a record in [suppliers] table
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        internal void Update(uint supplierId, string name = null, string contact = null)
        {
            if (name == null && contact == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");
            // Define sql command
            var command = new CommandDefinition(
                "update suppliers set " +
                ((name == null ? "" : "name = @name, ") +
                 (contact == null ? "" : "contact = @contact, ")).TrimEnd(' ', ',') +
                " where id_supplier = @supplierId",
                new {supplierId, name, contact});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [suppliers] table
        /// </summary>
        /// <param name="supplierId"></param>
        internal void Delete(uint supplierId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from suppliers where id_supplier = @supplierId",
                new {supplierId});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}