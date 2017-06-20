using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public class Battery : Item
    {
        public Battery(string name, decimal unitPrice, string brand,
            string capacity, string voltage, uint stockQty = 0) : base(name, ProductType.Battery, unitPrice, stockQty)
        {
            Brand = brand;
            Capacity = capacity;
            Voltage = voltage;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Battery()
        {
            ProductType = ProductType.Battery;
        }

        public string Capacity { get; set; }
        public string Voltage { get; set; }

        /// <summary>
        ///     Generate and return Item Name in following format
        ///     ([ItemCode] [Brand] [Model] [Capacity Ah] [Voltage V]
        /// </summary>
        /// <returns></returns>
        public override string GenerateName()
        {
            return
                (string.IsNullOrEmpty(ItemCode) ? "" : ItemCode.Trim() + " ") +
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Capacity) ? "" : Capacity.Trim() + "Ah ") +
                (string.IsNullOrEmpty(Voltage) ? "" : Voltage.Trim() + "V").Trim();
        }
    }
}