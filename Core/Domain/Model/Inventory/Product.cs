using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public abstract class Product
    {
        // Product Type obtained from derived class
        public ProductType ProductType =>
            this is Alloywheel
                ? ProductType.Alloywheel
                : this is Battery
                    ? ProductType.Battery
                    : this is Tyre
                        ? ProductType.Tyre
                        : ProductType.Service;

        public uint? Id { get; set; }

        // Properties and Methods below are overridden by inherited classes
        public abstract string Name { get; set; }

        public abstract override string ToString();
    }
}