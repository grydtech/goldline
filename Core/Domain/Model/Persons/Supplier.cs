using System.Collections.Generic;
using Core.Model.Products;

namespace Core.Model.Persons
{
    public class Supplier : Person
    {
        public Supplier(string name, string contact) : base(name, contact)
        {
            SuppliedItems = new List<Item>();
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Supplier()
        {
            SuppliedItems = new List<Item>();
        }

        public List<Item> SuppliedItems { get; set; }
    }
}