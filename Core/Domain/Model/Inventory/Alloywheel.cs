using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Alloywheel : Item
    {
        public Alloywheel(string name, decimal unitPrice, string brand,
            string dimension, uint stockQty = 0) : base(ProductType.Alloywheel, unitPrice, stockQty)
        {
            Name = name;
            Brand = brand;
            Dimension = dimension;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Alloywheel()
        {
            ProductType = ProductType.Alloywheel;
        }

        public string Dimension { get; set; }
        public override string Name { get; set; }
        public override string ToString()
        {
            return
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim());
        }
    }
}