using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Model.Enums;
using Core.Model.Persons;
using Core.Model.Products;
using Dapper;

namespace Core.Datalayer
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
                "insert ignore into suppliers (name_supplier, contact) values (@name_supplier, @contact)",
                new
                {
                    name_supplier = supplier.Name,
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
                "select id_supplier 'Id', name_supplier 'Name', Contact from suppliers " +
                "where name_supplier like @name_supplier " +
                "order by name_supplier",
                new {name_supplier = "%" + name + "%"});

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
                new CommandDefinition("select id_supplier 'Id', name_supplier 'Name', Contact from suppliers " +
                                      "order by name_supplier");

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
                "update suppliers set name_supplier = @name_supplier, contact = @contact " +
                "where id_supplier = @id_supplier",
                new
                {
                    id_supplier = supplier.Id,
                    name_supplier = supplier.Name,
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

        /// <summary>
        ///     Adds new supplied items to the supplier or ignore if already exists
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="suppliedItems"></param>
        internal void InsertSuppliedItems(uint supplierId, IEnumerable<Item> suppliedItems)
        {
            foreach (var suppliedItem in suppliedItems)
            {
                // Define sql command
                var command = new CommandDefinition(
                    "insert into suppliers_items (id_supplier, id_item) values (@id_supplier, @id_item)",
                    new
                    {
                        id_supplier = supplierId,
                        id_item = suppliedItem.Id
                    });

                // Execute sql command
                Connection.Execute(command);
            }
        }

        /// <summary>
        ///     Removes all supplied items of a supplier.
        ///     This method should be run when updating the supplied items of a supplier
        /// </summary>
        /// <param name="supplierId"></param>
        internal void ClearSuppliedItems(uint supplierId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from suppliers_items where id_supplier = @id_supplier",
                new
                {
                    id_supplier = supplierId
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of supplied items from a given supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        internal IEnumerable<Item> GetSuppliedItems(uint supplierId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select products.id_product 'Id', name_product 'Name', type_product-1 'ProductType', " +
                "qty_item 'Qty', unit_price 'UnitPrice' from suppliers_items " +
                "join items on suppliers_items.id_item = items.id_product " +
                "join products on items.id_product = products.id_product " +
                "where id_supplier = @id_supplier " +
                "order by type_product, name_product",
                new {id_supplier = supplierId});

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
                item.ProductType = (ProductType) o.ProductType;
                return item;
            });
        }
    }
}