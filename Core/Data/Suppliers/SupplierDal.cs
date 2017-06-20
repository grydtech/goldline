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
        ///     Inserts a new supplier into database.
        ///     The supplied items are not inserted through this
        /// </summary>
        /// <param name="supplier"></param>
        internal void InsertSupplier(Supplier supplier)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert ignore into suppliers (name, contact) values (@name, @contact)",
                new
                {
                    name = supplier.Name,
                    contact = supplier.Contact
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            supplier.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Returns a list of all suppliers matching given search parameters
        /// </summary>
        /// <param name="name">search by name</param>
        internal IEnumerable<Supplier> GetSuppliers(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_supplier 'Id', name 'Name', Contact from suppliers " +
                "where name like @name " +
                "order by name",
                new {name = "%" + name + "%"});

            // Execute sql command
            return Connection.Query<Supplier>(command);
        }

        /// <summary>
        ///     Returns a list of all suppliers
        /// </summary>
        internal IEnumerable<Supplier> GetAllSuppliers()
        {
            // Define sql command
            var command =
                new CommandDefinition("select id_supplier 'Id', name 'Name', Contact from suppliers " +
                                      "order by name");

            // Execute sql command
            return Connection.Query<Supplier>(command);
        }

        /// <summary>
        ///     Updates an existing supplier details in database.
        ///     The properties that will be updated are Name, Contact
        /// </summary>
        /// <param name="supplier"></param>
        internal void UpdateSupplierDetails(Supplier supplier)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update suppliers set name = @name, contact = @contact " +
                "where id_supplier = @id_supplier",
                new
                {
                    id_supplier = supplier.Id,
                    name = supplier.Name,
                    contact = supplier.Contact
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Removes existing supplier from database if constraints allow
        /// </summary>
        /// <param name="supplierId"></param>
        internal void RemoveSupplier(uint supplierId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from suppliers where id_supplier = @id_supplier",
                new {id_supplier = supplierId});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}