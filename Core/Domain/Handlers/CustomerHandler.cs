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
            if(customer == null) throw new ArgumentNullException(nameof(customer.Name), "Customer is null");
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer name is null");
            if (customer.Contact == null) throw new ArgumentNullException(nameof(customer.Contact), "Customer contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = Connector.GetConnection())
            {
                var customerDal = new CustomerDal(connection);
                customerDal.Insert(customer);
                customer.Id = customerDal.GetLastInsertId();
            }
        }

        /// <summary>
        ///     Search customers by name and give resulting list of customers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers(string name)
        {
            // Exception handling
            if (name == null) throw new ArgumentNullException(nameof(name), "Name is null");
            using (var connection = Connector.GetConnection())
            {
                return new CustomerDal(connection).Search(name);
            }
        }

        /// <summary>
        ///     Returns a list of all customers
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Customer> GetCustomers()
        {
            using (var connection = Connector.GetConnection())
            {
                return new CustomerDal(connection).Get();
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
        public void UpdateCustomer(Customer customer, string name = null,string nic = null, string contact = null)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer name is null");
            if (customer.Contact == null)
                throw new ArgumentNullException(nameof(customer.Contact), "Customer contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = Connector.GetConnection())
            {
                new CustomerDal(connection).Update(customer, name, nic, contact);
            }
        }

        /// <summary>
        ///     Adds a new vehicle to customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="vehicle"></param>
        public void AddNewCustomerVehicle(Customer customer, Vehicle vehicle)
        {
            // Exception handling
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle number is null");
            if (customer.Id == null)
                throw new ArgumentNullException(nameof(customer.Id), "Customer id is null");

            using (var connection = Connector.GetConnection())
            {
                new VehicleDal(connection).Insert(vehicle.Number, customer.Id.Value);
            }
        }

        /// <summary>
        ///     Mark the visit of a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public void MarkVehicleVisit(Vehicle vehicle)
        {
            // Exception handling
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle number is null");

            using (var connection = Connector.GetConnection())
            {
                new VehicleDal(connection).Update(vehicle.Number);
            }
        }

        /// <summary>
        ///     Gets a list of vehicles belonging to a customer
        /// </summary>
        /// <param name="customer"></param>
        public IEnumerable<Vehicle> GetVehiclesOfCustomer(Customer customer)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");

            using (var connection = Connector.GetConnection())
            {
                return new VehicleDal(connection).Search(customer.Id.Value);
            }
        }

        /// <summary>
        ///     Method to check if entered nic has correct format
        /// </summary>
        /// <param name="nic"></param>
        /// <returns></returns>
        public bool IsNicValid(string nic)
        {
            int n;
            var maxIndex = nic.Length - 1;
            return nic.Length == 10 && int.TryParse(nic.Substring(1, maxIndex - 1), out n) &&
                   (nic.Substring(maxIndex).ToUpper() == "V" || nic.Substring(maxIndex).ToUpper() == "X");
        }
    }
}