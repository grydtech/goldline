using System;
using System.Collections.Generic;
using Core.Data;
using Core.Model.Persons;

namespace Core.Model.Handlers
{
    public class CustomerHandler
    {
        /// <summary>
        ///     Adds a new customer
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer)
        {
            // Exception handling
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer name is null");
            if (customer.Contact == null)
                throw new ArgumentNullException(nameof(customer.Contact), "Customer contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                var customerDal = new CustomerDal(connection);
                customerDal.InsertCustomer(customer);
            }
        }

        /// <summary>
        ///     Search customers by name and give resulting list of customers
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomer(string name)
        {
            // Exception handling
            if (name == null) throw new ArgumentNullException(nameof(name), "Name is null");
            using (var connection = ConnectionManager.GetConnection())
            {
                return new CustomerDal(connection).GetCustomers(name);
            }
        }

        /// <summary>
        ///     Returns a list of all customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new CustomerDal(connection).GetAllCustomers();
            }
        }

        /// <summary>
        ///     Update customer with new attributes.
        ///     The properties that will be updated are: Name, Nic, Contact, DueAmount
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");
            if (customer.Name == null) throw new ArgumentNullException(nameof(customer.Name), "Customer name is null");
            if (customer.Contact == null)
                throw new ArgumentNullException(nameof(customer.Contact), "Customer contact is null");
            if (customer.Nic == null) throw new ArgumentNullException(nameof(customer.Nic), "Customer Nic is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                new CustomerDal(connection).UpdateCustomerDetails(customer);
            }
        }

        /// <summary>
        ///     Adds a new vehicle to customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="vehicle"></param>
        public void AddNewCustomerVehicle(Customer customer, CustomerVehicle vehicle)
        {
            // Exception handling
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle number is null");
            if (customer.Id == null)
                throw new ArgumentNullException(nameof(customer.Id), "Customer id is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                new CustomerDal(connection).InsertCustomerVehicle((uint) customer.Id, vehicle);
            }
        }

        /// <summary>
        ///     Mark the visit of a vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public void MarkVehicleVisit(CustomerVehicle vehicle)
        {
            // Exception handling
            if (vehicle.Number == null)
                throw new ArgumentNullException(nameof(vehicle.Number), "Vehicle number is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                new CustomerDal(connection).UpdateCustomerVehicleDate(vehicle);
            }
        }

        /// <summary>
        ///     Gets a list of vehicles belonging to a customer
        /// </summary>
        /// <param name="customer"></param>
        public IEnumerable<CustomerVehicle> GetVehiclesOfCustomer(Customer customer)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                return new CustomerDal(connection).GetCustomerVehicles(customer);
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