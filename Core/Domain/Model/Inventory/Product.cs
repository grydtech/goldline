using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public abstract class Product
    {
        protected Product(ProductType productType)
        {
            ProductType = productType;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        protected Product()
        {
        }

        public uint? Id { get; set; }
        public ProductType ProductType { get; protected set; }
        public abstract string Name { get; set; }
        public abstract override string ToString();
    }
}