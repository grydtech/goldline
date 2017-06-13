using Core.Model.Enums;

namespace Core.Model.Products
{
    public class Tyre : Item
    {
        public Tyre(string name, decimal unitPrice, string brand, string dimension,
            string country = null, uint stockQty = 0) : base(name, ProductType.Tyre, unitPrice, stockQty)
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

        /// <summary>
        ///     Generate and return Item Name in following format
        ///     ([ItemCode] [Dimension] [Brand] [Model] [Country])
        /// </summary>
        /// <returns></returns>
        public override string GenerateName()
        {
            return
                (string.IsNullOrEmpty(ItemCode) ? "" : ItemCode.Trim() + " ") +
                (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim() + " ") +
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Country) ? "" : "- " + Country.Trim()).Trim();
        }
    }
}