using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Core.Data;
using Core.Data.Inventory;
using Core.Domain.Enums;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Handlers
{
    public class ProductHandler
    {
        /// <summary>
        ///     Add any new product. Automatically determines the type of item and insert accordingly.
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var productDal = new ProductDal(connection);
                    productDal.Insert(product is Item ? ((Item)product).Model : product.Name, product.ProductType);
                    var id = productDal.GetLastInsertId();
                    product.Id = id;
                    if (product.Id == null) throw new ArgumentNullException(nameof(product.Id), "Product Id is null");

                    if (product is Item)
                    {
                        var item = (Item) product;
                        var itemDal = new ItemDal(connection);
                        itemDal.Insert(id, item.StockQty, item.UnitPrice);

                        if (item is Alloywheel)
                        {
                            var alloywheel = (Alloywheel) item;
                            var alloywheelDal = new AlloywheelDal(connection);
                            alloywheelDal.Insert(id, alloywheel.Brand, alloywheel.Dimension);
                        }

                        if (item is Tyre)
                        {
                            var tyre = (Tyre) item;
                            var tyreDal = new TyreDal(connection);
                            tyreDal.Insert(id, tyre.Brand, tyre.Dimension, tyre.Country);
                        }

                        if (item is Battery)
                        {
                            var battery = (Battery) item;
                            var batteryDal = new BatteryDal(connection);
                            batteryDal.Insert(id, battery.Brand, battery.Capacity, battery.Voltage);
                        }
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Returns products of a given type, matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts(string name = null, ProductType? productType = null)
        {
            using (var connection = Connector.GetConnection())
            {
                var productDal = new ProductDal(connection);
                return productDal.Search(name == null ? null : $"%{name}%", productType);
            }
        }

        /// <summary>
        ///     Returns items matching given search parameters
        ///     and optionally, the itemType
        /// </summary>
        /// <param name="name"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetItems(string name = null, ProductType? productType = null)
        {
            if (productType == ProductType.Service)
                throw new NotSupportedException("ProductType 'Service' is not applicable here");
            using (var connection = Connector.GetConnection())
            {
                var itemDal = new ItemDal(connection);
                return itemDal.Search(name == null ? null : $"%{name}%", productType);
            }
        }

        /// <summary>
        ///     Updates Product details
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        private static void Update(uint productId, string name, IDbConnection connection = null)
        {
            new ProductDal(connection).Update(productId, name);
        }

        /// <summary>
        ///     Updates Item details
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockQty"></param>
        /// <param name="unitPrice"></param>
        /// <param name="connection"></param>
        private static void Update(uint productId, uint? stockQty = null, decimal? unitPrice = null, IDbConnection connection = null)
        {
            if (stockQty == null && unitPrice == null)
                throw new ArgumentNullException(nameof(Update), "No parameters passed to update");
            new ItemDal(connection).Update(productId, stockQty, unitPrice);
        }


        public sealed class AlloywheelHandler : ProductHandler
        {
            /// <summary>
            ///     Updates Alloywheel details
            /// </summary>
            /// <param name="alloywheel"></param>
            /// <param name="name"></param>
            /// <param name="stockqty"></param>
            /// <param name="unitPrice"></param>
            /// <param name="brand"></param>
            /// <param name="dimension"></param>
            public void Update(Alloywheel alloywheel, string name = null, uint? stockqty = null,
                decimal? unitPrice = null, string brand = null, string dimension = null)
            {
                if(name == null && stockqty == null && unitPrice == null && brand == null && dimension == null)
                    throw new ArgumentNullException(nameof(Update), "No parameters passed to update");
                if (alloywheel.Id == null)
                    throw new ArgumentNullException(nameof(alloywheel.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = alloywheel.Id.Value;
                        if (name != null) Update(id, name, connection);
                        if (stockqty != null || unitPrice != null) Update(id, stockqty, unitPrice, connection);
                        if (brand != null || dimension != null)
                            new AlloywheelDal(connection).Update(alloywheel.Id.Value, brand, dimension);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new alloywheel brand or ignore if already exists
            /// </summary>
            /// <param name="brand"></param>
            /// <returns></returns>
            public void AddBrand(string brand)
            {
                using (var connection = Connector.GetConnection())
                {
                    new AlloywheelDal(connection).InsertBrand(brand);
                }
            }

            /// <summary>
            ///     Adds a new alloywheel dimension or ignore if already exists
            /// </summary>
            /// <param name="dimension"></param>
            /// <returns></returns>
            public void AddDimension(string dimension)
            {
                using (var connection = Connector.GetConnection())
                {
                    new AlloywheelDal(connection).InsertDimension(dimension);
                }
            }

            /// <summary>
            ///     Returns a list of all stored alloywheel brands
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetBrands()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new AlloywheelDal(connection).SearchBrand();
                }
            }

            /// <summary>
            ///     Returns a list of all stored alloywheel dimensions
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetDimensions()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new AlloywheelDal(connection).SearchDimension();
                }
            }
        }


        public sealed class BatteryHandler : ProductHandler
        {
            /// <summary>
            ///     Updates Battery details
            /// </summary>
            /// <param name="battery"></param>
            /// <param name="name"></param>
            /// <param name="stockqty"></param>
            /// <param name="unitPrice"></param>
            /// <param name="brand"></param>
            /// <param name="capacity"></param>
            /// <param name="voltage"></param>
            public void Update(Battery battery, string name = null, uint? stockqty = null, decimal? unitPrice = null,
                string brand = null, string capacity = null, string voltage = null)
            {
                if (name == null && stockqty == null && unitPrice == null && brand == null && capacity == null && voltage == null)
                    throw new ArgumentNullException(nameof(Update), "No parameters passed to update");
                if (battery.Id == null)
                    throw new ArgumentNullException(nameof(battery.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = battery.Id.Value;
                        if (name != null) Update(id, name, connection);
                        if (stockqty != null || unitPrice != null) Update(id, stockqty, unitPrice, connection);
                        if (brand != null || capacity != null || voltage != null)
                            new BatteryDal(connection).Update(battery.Id.Value, brand, capacity, voltage);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new battery brand or ignore if already exists
            /// </summary>
            /// <param name="brand"></param>
            /// <returns></returns>
            public void AddBrand(string brand)
            {
                using (var connection = Connector.GetConnection())
                {
                    new BatteryDal(connection).InsertBrand(brand);
                }
            }

            /// <summary>
            ///     Returns a list of all stored battery brands
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetBrands()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new BatteryDal(connection).SearchBrand();
                }
            }
        }


        public sealed class TyreHandler : ProductHandler
        {
            /// <summary>
            ///     Updates Tyre details
            /// </summary>
            /// <param name="tyre"></param>
            /// <param name="name"></param>
            /// <param name="stockqty"></param>
            /// <param name="unitPrice"></param>
            /// <param name="brand"></param>
            /// <param name="dimension"></param>
            /// <param name="country"></param>
            public void Update(Tyre tyre, string name = null, uint? stockqty = null, decimal? unitPrice = null,
                string brand = null, string dimension = null, string country = null)
            {
                if (name == null && stockqty == null && unitPrice == null && brand == null && dimension == null && country == null)
                    throw new ArgumentNullException(nameof(Update), "No parameters passed to update");
                if (tyre.Id == null)
                    throw new ArgumentNullException(nameof(tyre.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = tyre.Id.Value;
                        if (name != null) Update(id, name, connection);
                        if (stockqty != null || unitPrice != null) Update(id, stockqty, unitPrice, connection);
                        if (brand != null || dimension != null || country != null)
                            new TyreDal(connection).Update(tyre.Id.Value, brand, dimension, country);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new tyre brand or ignore if already exists
            /// </summary>
            /// <param name="brand"></param>
            /// <returns></returns>
            public void AddBrand(string brand)
            {
                using (var connection = Connector.GetConnection())
                {
                    new TyreDal(connection).InsertBrand(brand);
                }
            }

            /// <summary>
            ///     Adds a new tyre dimension or ignore if already exists
            /// </summary>
            /// <param name="dimension"></param>
            /// <returns></returns>
            public void AddDimension(string dimension)
            {
                using (var connection = Connector.GetConnection())
                {
                    new TyreDal(connection).InsertDimension(dimension);
                }
            }

            /// <summary>
            ///     Adds a new country or ignore if already exists
            /// </summary>
            /// <param name="country"></param>
            /// <returns></returns>
            public void AddCountry(string country)
            {
                using (var connection = Connector.GetConnection())
                {
                    new TyreDal(connection).InsertCountry(country);
                }
            }

            /// <summary>
            ///     Returns a list of all stored tyre brands
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetBrands()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new TyreDal(connection).SearchBrand();
                }
            }

            /// <summary>
            ///     Returns a list of all stored tyre dimensions
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetDimensions()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new TyreDal(connection).SearchDimension();
                }
            }

            /// <summary>
            ///     Returns a list of all stored countries
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetCountries()
            {
                using (var connection = Connector.GetConnection())
                {
                    return new TyreDal(connection).SearchCountry();
                }
            }
        }


        public sealed class ServiceHandler : ProductHandler
        {
            /// <summary>
            ///     Updates Tyre details
            /// </summary>
            /// <param name="service"></param>
            /// <param name="name"></param>
            public void Update(Service service, string name = null)
            {
                if (name == null) throw new ArgumentNullException(nameof(name), "No parameters passed to update");
                if (service?.Id == null)
                    throw new ArgumentNullException(nameof(service.Id), "Updating component does not have Id");
                using (var connection = Connector.GetConnection())
                {
                    Update(service.Id.Value, name, connection);
                }
            }
        }
    }
}