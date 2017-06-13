using System;
using System.Collections.Generic;
using Core.Data;
using Core.Model.Enums;
using Core.Model.Products;

namespace Core.Model.Handlers
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
            using (var connection = ConnectionManager.GetConnection())
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