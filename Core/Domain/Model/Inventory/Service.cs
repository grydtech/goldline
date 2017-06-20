using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public class Service : Product
    {
        public Service(string name) : base(name, ProductType.Service)
        {
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Service()
        {
            ProductType = ProductType.Service;
        }
    }
}