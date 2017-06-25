using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public sealed class Alloywheel : Item
    {
        public Alloywheel(string brand, string model, uint stockQty,
            decimal unitPrice, string dimension) : base(brand, model, stockQty, unitPrice)
        {
            Dimension = dimension;
        }

        public Alloywheel()
        {
        }

        public string Dimension { get; set; }
        public override string Name
        {
            get { return ToString(); }
            set { }
        }

        public override string ToString()
        {
            return
                (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim());
        }

        public override bool Validate()
        {
            return !(string.IsNullOrWhiteSpace(Brand) || string.IsNullOrWhiteSpace(Model) ||
                     string.IsNullOrWhiteSpace(Dimension));
        }
    }
}