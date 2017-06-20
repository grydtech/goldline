using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public class Alloywheel : Item
    {
        public Alloywheel(string name, decimal unitPrice, string brand,
            string dimension, uint stockQty = 0) : base(name, ProductType.Alloywheel, unitPrice, stockQty)
        {
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

        /// <summary>
        ///     Generate and return Item Name in following format
        ///     ([ItemCode] [Dimension] [Brand] [Model])
        /// </summary>
        /// <returns></returns>
        public override string GenerateName()
        {
            return
                (string.IsNullOrEmpty(ItemCode) ? "" : ItemCode.Trim() + " ") +
                (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim() + " ") +
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim()).Trim();
        }
    }
}