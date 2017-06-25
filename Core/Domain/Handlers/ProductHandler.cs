﻿using System;
using System.Collections.Generic;
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
                    productDal.Insert(product.Name, product.ProductType);
                    var id = productDal.GetLastInsertId();
                    product.Id = id;
                    if (product.Id == null) throw new ArgumentNullException(nameof(product.Id), "Product Id is null");

                    if (product is Item item)
                    {
                        var itemDal = new ItemDal(connection);
                        itemDal.Insert(id, item.StockQty, item.UnitPrice);

                        if (item is Alloywheel alloywheel)
                        {
                            var alloywheelDal = new AlloywheelDal(connection);
                            alloywheelDal.Insert(id, alloywheel.Brand, alloywheel.Dimension);
                        }

                        if (item is Tyre tyre)
                        {
                            var tyreDal = new TyreDal(connection);
                            tyreDal.Insert(id, tyre.Brand, tyre.Dimension, tyre.Country);
                        }

                        if (item is Battery battery)
                        {
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
            if (productType == ProductType.Service) throw new NotSupportedException("ProductType 'Service' is not applicable here");
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
        private void UpdateProduct(uint productId, string name = null)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    new ProductDal(connection).Update(productId, name);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates Item details
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockQty"></param>
        /// <param name="unitPrice"></param>
        private void UpdateItem(uint productId, uint? stockQty = null, decimal? unitPrice = null)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    new ItemDal(connection).Update(productId, stockQty, unitPrice);
                }
                scope.Complete();
            }
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
            public void UpdateAlloywheel(Alloywheel alloywheel, string name = null, uint? stockqty = null, decimal? unitPrice = null, string brand = null, string dimension = null)
            {
                if (alloywheel.Id == null) throw new ArgumentNullException(nameof(alloywheel.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = alloywheel.Id.Value;
                        UpdateProduct(id, name);
                        UpdateItem(id, stockqty, unitPrice);
                        new AlloywheelDal(connection).Update(alloywheel.Id.Value, brand, dimension);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new alloywheel brand or ignore if already exists
            /// </summary>
            /// <param name="brandName"></param>
            /// <returns></returns>
            public void AddAlloywheelBrand(string brandName)
            {
                using (var connection = Connector.GetConnection())
                {
                    new AlloywheelDal(connection).InsertBrand(brandName);
                }
            }

            /// <summary>
            ///     Adds a new alloywheel dimension or ignore if already exists
            /// </summary>
            /// <param name="alloywheelDimension"></param>
            /// <returns></returns>
            public void AddAlloywheelDimension(string alloywheelDimension)
            {
                using (var connection = Connector.GetConnection())
                {
                    new AlloywheelDal(connection).InsertDimension(alloywheelDimension);
                }
            }

            /// <summary>
            ///     Returns a list of all stored alloywheel brands
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetAlloywheelBrands()
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
            public IEnumerable<string> GetAlloywheelDimensions()
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
            public void UpdateBattery(Battery battery, string name = null, uint? stockqty = null, decimal? unitPrice = null, string brand = null, string capacity = null, string voltage = null)
            {
                if (battery.Id == null) throw new ArgumentNullException(nameof(battery.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = battery.Id.Value;
                        UpdateProduct(id, name);
                        UpdateItem(id, stockqty, unitPrice);
                        new BatteryDal(connection).Update(battery.Id.Value, brand, capacity, voltage);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new battery brand or ignore if already exists
            /// </summary>
            /// <param name="batteryBrand"></param>
            /// <returns></returns>
            public void AddBatteryBrand(string batteryBrand)
            {
                using (var connection = Connector.GetConnection())
                {
                    new BatteryDal(connection).InsertBrand(batteryBrand);
                }
            }

            /// <summary>
            ///     Returns a list of all stored battery brands
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetBatteryBrands()
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
            public void UpdateTyre(Tyre tyre, string name = null, uint? stockqty = null, decimal? unitPrice = null, string brand = null, string dimension = null, string country = null)
            {
                if (tyre.Id == null) throw new ArgumentNullException(nameof(tyre.Id), "Updating component does not have Id");
                using (var scope = new TransactionScope())
                {
                    using (var connection = Connector.GetConnection())
                    {
                        var id = tyre.Id.Value;
                        UpdateProduct(id, name);
                        UpdateItem(id, stockqty, unitPrice);
                        new TyreDal(connection).Update(tyre.Id.Value, brand, dimension, country);
                    }
                    scope.Complete();
                }
            }

            /// <summary>
            ///     Adds a new tyre brand or ignore if already exists
            /// </summary>
            /// <param name="tyreBrand"></param>
            /// <returns></returns>
            public void AddTyreBrand(string tyreBrand)
            {
                using (var connection = Connector.GetConnection())
                {
                    new TyreDal(connection).InsertBrand(tyreBrand);
                }
            }

            /// <summary>
            ///     Adds a new tyre dimension or ignore if already exists
            /// </summary>
            /// <param name="tyreDimension"></param>
            /// <returns></returns>
            public void AddTyreDimension(string tyreDimension)
            {
                using (var connection = Connector.GetConnection())
                {
                    new TyreDal(connection).InsertDimension(tyreDimension);
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
            public IEnumerable<string> GetTyreBrands()
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
            public IEnumerable<string> GetTyreDimensions()
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
            public void UpdateService(Service service, string name = null)
            {
                if (service.Id == null)
                    throw new ArgumentNullException(nameof(service.Id), "Updating component does not have Id");
                UpdateProduct(service.Id.Value, name);
            }
        }
    }
}