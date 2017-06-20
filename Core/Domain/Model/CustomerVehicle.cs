using System;

namespace Core.Model
{
    public class CustomerVehicle
    {
        public CustomerVehicle(string number, uint customerId)
        {
            Number = number;
            CustomerId = customerId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public CustomerVehicle()
        {
        }

        public string Number { get; set; }
        public uint CustomerId { get; set; }
        public DateTime LastVisitDate { get; set; }
    }
}