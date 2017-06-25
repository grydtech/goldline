using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Battery : Item
    {
        public Battery(string brand, string model, uint stockQty,
            decimal unitPrice, string capacity, string voltage) : base(brand, model, stockQty, unitPrice)
        {
            Capacity = capacity;
            Voltage = voltage;
        }

        public Battery()
        {
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