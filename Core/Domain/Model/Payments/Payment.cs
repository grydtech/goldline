using System;

namespace Core.Model.Payments
{
    public abstract class Payment
    {
        public Payment(decimal amount, string note)
        {
            Amount = amount;
            Note = note;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Payment()
        {
        }

        public uint? Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public uint UserId { get; set; }
    }
}