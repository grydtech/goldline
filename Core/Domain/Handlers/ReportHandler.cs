using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Inventory;
using Core.Domain.Enums;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Handlers
{
    public class ReportHandler
    {
        /// <summary>
        ///     Gets list of products of given type to view as a catalog
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetCatalog(ProductType productType)
        {
            using (var connection = Connector.GetConnection())
            {
                var productDal = new ProductDal(connection);
                switch (productType)
                {
                    case ProductType.Alloywheel:
                        return productDal.GetAllAlloywheels();
                    case ProductType.Battery:
                        return productDal.GetAllBatteries();
                    case ProductType.Tyre:
                        return productDal.GetAllTyres();
                    case ProductType.Service:
                        return productDal.GetAllServices();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(productType), productType,
                            "ProductType not in enum");
                }
            }
        }
    }
}