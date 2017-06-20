using System;

namespace Core.Domain.Model
{
    public abstract class Payment
    {
        protected Payment(DateTime date, decimal amount, string note)
        {
            Date = date;
            Amount = amount;
            Note = note;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        protected Payment()
        {
        }

        public uint? Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}