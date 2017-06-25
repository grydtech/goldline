using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Customers;
using Core.Domain.Model.Customers;

namespace Core.Domain.Handlers
{
    public class CustomerHandler
    {
        /// <summary>
        ///     Adds a new customer
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            // Exception handling
            if (customer == null) throw new ArgumentNullException(nameof(customer), "Customer is null");
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer Name is null");
            if (customer.Contact == null)
                throw new ArgumentNullException(nameof(customer.Contact), "Customer Contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = Connector.GetConnection())
            {
                var customerDal = new CustomerDal(connection);
                customerDal.Insert(customer.Name, customer.Nic, customer.Contact);
                customer.Id = customerDal.GetLastInsertId();
            }
        }

        /// <summary>
        ///     Search customers by name and give resulting list of customers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers(string name = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new CustomerDal(connection).Search(name == null ? null : $"%{name}%");
            }
        }

        /// <summary>
        ///     Update customer with new attributes.
        ///     The properties that will be updated are: Name, Nic, Contact, DueAmount
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="name"></param>
        /// <param name="nic"></param>
        /// <param name="contact"></param>
        public void UpdateCustomer(Customer customer, string name = null, string nic = null,
            string contact = null)
        {
            // Exception handling
            if (customer == null) throw new ArgumentNullException(nameof(customer), "Customer is null");
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer Name is null");
            if (customer.Contact == null)
                throw new ArgumentNullException(nameof(customer.Contact), "Customer Contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = Connector.GetConnection())
            {
                new CustomerDal(connection).Update(customer.Id.Value, name, nic, contact);
                customer.Name = name ?? customer.Name;
                customer.Nic = nic ?? customer.Nic;
                customer.Contact = contact ?? customer.Contact;
            }
        }

        /// <summary>
        ///     Adds a new vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public void AddVehicle(Vehicle vehicle)
        {
            // Exception handling
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle), "Vehicle is null");
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle Number is null");
            if (vehicle.CustomerId == default(uint))
                throw new ArgumentNullException(nameof(vehicle.CustomerId), "Vehicle CustomerId is not specified");

            using (var connection = Connector.GetConnection())
            {
                new VehicleDal(connection).Insert(vehicle.Number, vehicle.CustomerId);
                vehicle.LastSeenDate = DateTime.Now;
            }
        }

        /// <summary>
        ///     Mark the visit of a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="lastSeen"></param>
        public void UpdateVehicle(Vehicle vehicle, DateTime lastSeen)
        {
            // Exception handling
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle), "Vehicle is null");
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle number is null");
            if (lastSeen == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Last Seen DateTime is null");

            using (var connection = Connector.GetConnection())
            {
                new VehicleDal(connection).Update(vehicle.Number, lastSeen);
                vehicle.LastSeenDate = lastSeen;
            }
        }

        /// <summary>
        ///     Gets a list of vehicles belonging to a customer
        /// </summary>
        /// <param name="customerId"></param>
        public IEnumerable<Vehicle> GetVehicles(uint customerId)
        {
            using (var connection = Connector.GetConnection())
            {
                return new VehicleDal(connection).Search(customerId);
            }
        }
    }
}