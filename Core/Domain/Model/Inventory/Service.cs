using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Service : Product
    {
        public Service(string name) : base(ProductType.Service)
        {
            Name = name;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Service()
        {
            ProductType = ProductType.Service;
        }

        public override string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}