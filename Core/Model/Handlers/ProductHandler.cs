using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Datalayer;
using Core.Model.Enums;
using Core.Model.Products;

namespace Core.Model.Handlers
{
    public class ProductHandler
    {
        /// <summary>
        ///     Add any new product. Automatically determines the type of item and insert accordingly.
        /// </summary>
        /// <param name="product"></param>
        public bool AddProduct(Product product)
        {
            switch (product.ProductType)
            {
                case ProductType.Alloywheel:
                    AddAlloywheel((Alloywheel) product);
                    break;
                case ProductType.Battery:
                    AddBattery((Battery) product);
                    break;
                case ProductType.Tyre:
                    AddTyre((Tyre) product);
                    break;
                case ProductType.Service:
                    AddService((Service) product);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(product.ProductType),
                        "ProductType not in enum");
            }
            return true;
        }

        /// <summary>
        ///     Update any existing product. Automatically determines the type of item and update accordingly.
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            switch (product.ProductType)
            {
                case ProductType.Alloywheel:
                    UpdateAlloywheel((Alloywheel) product);
                    break;
                case ProductType.Battery:
                    UpdateBattery((Battery) product);
                    break;
                case ProductType.Tyre:
                    UpdateTyre((Tyre) product);
                    break;
                case ProductType.Service:
                    UpdateService((Service) product);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(product.ProductType),
                        "ProductType not in enum");
            }
        }

        /// <summary>
        ///     Returns products of a given type, matching given search parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProducts(string name, ProductType? productType = null)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                var productDal = new ProductDal(connection);
                switch (productType)
                {
                    case ProductType.Alloywheel:
                        return productDal.GetAlloywheels(name);
                    case ProductType.Battery:
                        return productDal.GetBatteries(name);
                    case ProductType.Tyre:
                        return productDal.GetTyres(name);
                    case ProductType.Service:
                        return productDal.GetServices(name);
                    case null:
                        return productDal.GetProducts(name);
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(productType),
                            "ProductType not in enum");
                }
            }
        }

        /// <summary>
        ///     Returns items matching given search parameters
        ///     and optionally, the itemType
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetItems(string name, ItemType? itemType = null)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                var productDal = new ProductDal(connection);
                switch (itemType)
                {
                    case ItemType.Alloywheel:
                        return productDal.GetAlloywheels(name);
                    case ItemType.Battery:
                        return productDal.GetBatteries(name);
                    case ItemType.Tyre:
                        return productDal.GetTyres(name);
                    case null:
                        return productDal.GetItems(name);
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(itemType),
                            "ItemType not in enum");
                }
            }
        }

        /// <summary>
        ///     Adds a new Tyre and assigns id to it
        /// </summary>
        /// <param name="tyre"></param>
        private void AddTyre(Tyre tyre)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).InsertTyre(tyre);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Adds a new Battery and assigns id to it
        /// </summary>
        /// <param name="battery"></param>
        private void AddBattery(Battery battery)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).InsertBattery(battery);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Adds a new Alloywheel and assigns id to it
        /// </summary>
        /// <param name="alloywheel"></param>
        private void AddAlloywheel(Alloywheel alloywheel)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).InsertAlloywheel(alloywheel);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Adds a new Service and assigns id to it
        /// </summary>
        /// <param name="service"></param>
        private void AddService(Service service)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).InsertService(service);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates Tyre details
        /// </summary>
        /// <param name="tyre"></param>
        private void UpdateTyre(Tyre tyre)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).UpdateTyre(tyre);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates Battery details
        /// </summary>
        /// <param name="battery"></param>
        private void UpdateBattery(Battery battery)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).UpdateBattery(battery);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates Alloywheel details
        /// </summary>
        /// <param name="alloywheel"></param>
        private void UpdateAlloywheel(Alloywheel alloywheel)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).UpdateAlloywheel(alloywheel);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates Tyre details
        /// </summary>
        /// <param name="service"></param>
        private void UpdateService(Service service)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new ProductDal(connection).UpdateService(service);
                }
                scope.Complete();
            }
        }
    }
}