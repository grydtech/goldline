using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public abstract class Product
    {
        protected Product(string name, ProductType productType)
        {
            Name = name;
            ProductType = productType;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        protected Product()
        {
        }

        public uint? Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
    }
}