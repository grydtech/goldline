using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Model.Enums;
using Core.Model.Products;
using Dapper;

namespace Core.Data
{
    internal class ProductDal : Dal
    {
        internal ProductDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a tyre into database and assigns its Id
        /// </summary>
        /// <param name="tyre"></param>
        internal void InsertTyre(Tyre tyre)
        {
            // Insert record into items and assign its Id
            InsertItem(tyre);

            // Define sql command
            var command = new CommandDefinition(
                "insert into tyres (id_product, brand, dimension, country) values (@id_product, @brand, @dimension, @country)",
                new
                {
                    id_product = tyre.Id,
                    brand = tyre.Brand,
                    dimension = tyre.Dimension,
                    country = tyre.Country
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts an alloywheel into database and assigns its Id
        /// </summary>
        /// <param name="alloywheel"></param>
        internal void InsertAlloywheel(Alloywheel alloywheel)
        {
            // Insert record into items and assign its Id
            InsertItem(alloywheel);

            // Define sql command
            var command = new CommandDefinition(
                "insert into alloywheels (id_product, brand, dimension) values (@id_product, @brand, @dimension)",
                new
                {
                    id_product = alloywheel.Id,
                    brand = alloywheel.Brand,
                    dimension = alloywheel.Dimension
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a battery into database and assigns its Id
        /// </summary>
        /// <param name="battery"></param>
        internal void InsertBattery(Battery battery)
        {
            // Insert record into items and assign its Id
            InsertItem(battery);

            // Define sql command
            var command = new CommandDefinition(
                "insert into batteries (id_product, brand, capacity, voltage) values (@id_product, @brand, @capacity, @voltage)",
                new
                {
                    id_product = battery.Id,
                    brand = battery.Brand,
                    capacity = uint.Parse(battery.Capacity),
                    voltage = uint.Parse(battery.Voltage)
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a service into databse and assigns its Id
        /// </summary>
        /// <param name="service"></param>
        internal void InsertService(Service service)
        {
            // Insert record into products and assign its Id
            InsertProduct(service);
        }

        /// <summary>
        ///     Updates an existing tyre in database
        /// </summary>
        /// <param name="tyre"></param>
        internal void UpdateTyre(Tyre tyre)
        {
            // Update record in items
            UpdateItem(tyre);

            // Define sql command
            var command = new CommandDefinition(
                "update tyres set brand = @brand, dimension = @dimension, country = @country " +
                "where id_product = @id_product",
                new
                {
                    id_product = tyre.Id,
                    brand = tyre.Brand,
                    dimension = tyre.Dimension,
                    country = tyre.Country
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates an existing alloywheel in database
        /// </summary>
        /// <param name="alloywheel"></param>
        internal void UpdateAlloywheel(Alloywheel alloywheel)
        {
            // Update record in items
            UpdateItem(alloywheel);

            // Define sql command
            var command = new CommandDefinition(
                "update alloywheels set brand = @brand, dimension = @dimension " +
                "where id_product = @id_product",
                new
                {
                    id_product = alloywheel.Id,
                    brand = alloywheel.Brand,
                    dimension = alloywheel.Dimension
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates an existing battery in database
        /// </summary>
        /// <param name="battery"></param>
        internal void UpdateBattery(Battery battery)
        {
            // Update record in items
            UpdateItem(battery);

            // Define sql command
            var command = new CommandDefinition(
                "update batteries set brand = @brand, capacity = @capacity, voltage = @voltage " +
                "where id_product = @id_product",
                new
                {
                    id_product = battery.Id,
                    brand = battery.Brand,
                    capacity = uint.Parse(battery.Capacity),
                    voltage = uint.Parse(battery.Voltage)
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates an existing service in database
        /// </summary>
        /// <param name="service"></param>
        internal void UpdateService(Service service)
        {
            // Update record in products
            UpdateProduct(service);
        }

        /// <summary>
        ///     Inserts an item into database and assigns its Id
        /// </summary>
        /// <param name="item"></param>
        private void InsertItem(Item item)
        {
            // Insert record into products and assign its Id
            InsertProduct(item);

            // Define sql command
            var command = new CommandDefinition(
                "insert into items (id_product, qty_stocks, unit_price) values (LAST_INSERT_ID(), @qty_stocks, @unit_price)",
                new
                {
                    qty_stocks = item.StockQty,
                    unit_price = item.UnitPrice
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates an existing item in database
        /// </summary>
        /// <param name="item"></param>
        private void UpdateItem(Item item)
        {
            // Update record in products
            UpdateProduct(item);

            // Define sql command
            var command = new CommandDefinition(
                "update items set qty_stocks = @qty_stocks, unit_price = @unit_price " +
                "where id_product = @id_product",
                new
                {
                    id_product = item.Id,
                    qty_stocks = item.StockQty,
                    unit_price = item.UnitPrice
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a product into database and assigns its Id
        /// </summary>
        /// <param name="product"></param>
        private void InsertProduct(Product product)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into products (name_product, type_product) values (@name_product, @type_product)",
                new
                {
                    name_product = product.Name,
                    type_product = product.ProductType.ToString()
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            product.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Updates an existing product in database
        /// </summary>
        /// <param name="product"></param>
        private void UpdateProduct(Product product)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update products set name_product = @name_product, type_product = @type_product " +
                "where id_product = @id_product",
                new
                {
                    id_product = product.Id,
                    name_product = product.Name,
                    type_product = product.ProductType.ToString()
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of all tyres in the database.
        ///     This method is used to show tyre catalog
        /// </summary>
        internal IEnumerable<Tyre> GetAllTyres()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension, Country from tyres " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "order by name_product");

            // Execute sql command
            return Connection.Query<Tyre>(command);
        }

        /// <summary>
        ///     Returns a list of all alloywheels in the database.
        ///     This method is used to show alloywheel catalog
        /// </summary>
        internal IEnumerable<Alloywheel> GetAllAlloywheels()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension from alloywheels " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "order by name_product");

            // Execute sql command
            return Connection.Query<Alloywheel>(command);
        }

        /// <summary>
        ///     Returns a list of all batteries in the database.
        ///     This method is used to show battery catalog
        /// </summary>
        internal IEnumerable<Battery> GetAllBatteries()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Capacity, Voltage from batteries " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "order by name_product");

            // Execute sql command
            return Connection.Query<Battery>(command);
        }

        /// <summary>
        ///     Returns a list of all services in database
        ///     This method is used to show service catalog
        /// </summary>
        internal IEnumerable<Service> GetAllServices()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name' from products " +
                "where type_product = 'Service' " +
                "order by name_product");

            // Execute sql command
            return Connection.Query<Service>(command);
        }

        /// <summary>
        ///     Returns a list of tyres matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Tyre> GetTyres(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension, Country from tyres " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "where name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

            // Execute sql command
            return Connection.Query<Tyre>(command);
        }

        /// <summary>
        ///     Returns a list of alloywheels matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Alloywheel> GetAlloywheels(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Dimension from alloywheels " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "where name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

            // Execute sql command
            return Connection.Query<Alloywheel>(command);
        }

        /// <summary>
        ///     Returns a list of batteries matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Battery> GetBatteries(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', qty_stocks 'StockQty', " +
                "unit_price 'UnitPrice', Brand, Capacity, Voltage from batteries " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "where name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

            // Execute sql command
            return Connection.Query<Battery>(command);
        }

        /// <summary>
        ///     Returns a list of services matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Service> GetServices(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name' from products " +
                "where type_product = 'Service' and name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

            // Execute sql command
            return Connection.Query<Service>(command);
        }

        /// <summary>
        ///     Returns a list of products matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Product> GetProducts(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_product 'Id', name_product 'Name', type_product-1 'ProductType' from products " +
                "where name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

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
                        product = new Tyre();
                        break;
                    default:
                        return null;
                }

                product.Id = (uint) o.Id;
                product.Name = o.Name;
                product.ProductType = o.ProductType;
                return product;
            });
        }

        /// <summary>
        ///     Returns a list of items matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        internal IEnumerable<Item> GetItems(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select products.id_product 'Id', name_product 'Name', type_product-1 'ProductType', " +
                "qty_stocks 'StockQty', unit_price 'UnitPrice' from products " +
                "join items USING(id_product) " +
                "join products USING(id_product) " +
                "where name_product like @name_product " +
                "order by name_product",
                new {name_product = "%" + name + "%"});

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
                item.StockQty = o.StockQty;
                item.UnitPrice = o.UnitPrice;
                return item;
            });
        }
    }
}