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

        // Name Property is overridden by Item Classes
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}