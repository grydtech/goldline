using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Tyre : Item
    {
        public Tyre(string name, decimal unitPrice, string brand, string dimension,
            string country = null, uint stockQty = 0) : base(ProductType.Tyre, unitPrice, stockQty)
        {
            Brand = brand;
            Dimension = dimension;
            Country = country;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Tyre()
        {
            ProductType = ProductType.Tyre;
        }

        public string Dimension { get; set; }
        public string Country { get; set; }

        public override string Name { get; set; }
        public override string ToString()
        {
            return
                (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim() + " ") +
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Country) ? "" : "- " + Country.Trim()).Trim();
        }
    }
}