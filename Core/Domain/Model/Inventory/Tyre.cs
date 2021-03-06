﻿namespace Core.Domain.Model.Inventory
{
    public sealed class Tyre : Item
    {
        public Tyre(string brand, string model, uint stockQty, decimal unitPrice, string dimension,
            string country) : base(brand, model, stockQty, unitPrice)
        {
            Dimension = dimension;
            Country = country;
        }

        public Tyre()
        {
        }

        public string Dimension { get; set; }
        public string Country { get; set; }

        public override string Name => (string.IsNullOrEmpty(Dimension) ? "" : Dimension.Trim() + " ") +
                                       (string.IsNullOrEmpty(Brand) ? "" : Brand.Trim() + " ") +
                                       (string.IsNullOrEmpty(Model) ? "" : Model.Trim() + " ") +
                                       (string.IsNullOrEmpty(Country) ? "" : "- " + Country.Trim()).Trim();

        public override bool Validate()
        {
            return !(string.IsNullOrWhiteSpace(Brand) || string.IsNullOrWhiteSpace(Model) ||
                     string.IsNullOrWhiteSpace(Dimension) || string.IsNullOrWhiteSpace(Country));
        }
    }
}