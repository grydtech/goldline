using Core.Model.Enums;

namespace Core.Model.Products
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