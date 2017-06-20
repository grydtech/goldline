using Core.Model.Enums;

namespace Core.Model.Products
{
    public abstract class Product
    {
        public Product(string name, ProductType productType)
        {
            Name = name;
            ProductType = productType;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Product()
        {
        }

        public uint? Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
    }
}