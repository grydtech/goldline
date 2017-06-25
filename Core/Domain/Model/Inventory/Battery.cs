using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Battery : Item
    {
        public Battery(string name, decimal unitPrice, string brand,
            string capacity, string voltage, uint stockQty = 0) : base(ProductType.Battery, unitPrice, stockQty)
        {
            Name = name;
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

        public override string Name { get; set; }
        public override string ToString()
        {
            return
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Capacity) ? "" : Capacity.Trim() + "Ah ") +
                (string.IsNullOrEmpty(Voltage) ? "" : Voltage.Trim() + "V").Trim();
        }
    }
}