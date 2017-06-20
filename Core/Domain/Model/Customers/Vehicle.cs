using System;

namespace Core.Domain.Model.Customers
{
    public class Vehicle
    {
        public Vehicle(string number, uint customerId)
        {
            Number = number;
            CustomerId = customerId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Vehicle()
        {
        }

        public string Number { get; set; }
        public uint CustomerId { get; set; }
        public DateTime LastVisitDate { get; set; }
    }
}