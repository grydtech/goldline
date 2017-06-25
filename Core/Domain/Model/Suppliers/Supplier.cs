namespace Core.Domain.Model.Suppliers
{
    public class Supplier : Person
    {
        public Supplier(string name, string contact) : base(name, contact)
        {
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Supplier()
        {  
        }
    }
}