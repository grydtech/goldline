using System.Collections.Generic;
using System.Data;
using Core.Model;
using Core.Model.Persons;
using Dapper;

namespace Core.Data
{
    internal class CustomerDal : Dal
    {
        internal CustomerDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a customer into database and assign its Id
        /// </summary>
        /// <param name="customer"></param>
        internal void InsertCustomer(Customer customer)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers (name, nic, contact) values (@name, @nic, @contact)",
                new
                {
                    name = customer.Name,
                    nic = customer.Nic,
                    contact = customer.Contact
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customer.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Returns a list of customers matching the search parameters
        /// </summary>
        /// <param name="name">search by name</param>
        /// <returns></returns>
        internal IEnumerable<Customer> GetCustomers(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_customer 'Id', name 'Name', nic 'Nic', contact 'Contact', credit_amount(id_customer) 'DueAmount' from customers " +
                "where name like @name " +
                "order by name",
                new
                {
                    name = "%" + name + "%"
                });

            // Execute sql command
            return Connection.Query<Customer>(command);
        }

        /// <summary>
        ///     Returns a list of all customers in the database
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Customer> GetAllCustomers()
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_customer 'Id', name 'Name', nic 'Nic', contact 'Contact', credit_amount(id_customer) 'DueAmount' from customers " +
                "order by name");

            // Execute sql command
            return Connection.Query<Customer>(command);
        }

        /// <summary>
        ///     Updates a customer in the database.
        ///     The properties that will be updated are: Name, Nic, Contact
        /// </summary>
        /// <param name="customer"></param>
        internal void UpdateCustomerDetails(Customer customer)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update customers set name = @name, nic = @nic, contact = @contact " +
                "where id_customer = @id_customer",
                new
                {
                    id_customer = customer.Id,
                    name_customer = customer.Name,
                    nic = customer.Nic,
                    contact = customer.Contact
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new customer vehicle into database or update last visited date if record exists
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerVehicle"></param>
        internal void InsertCustomerVehicle(uint customerId, CustomerVehicle customerVehicle)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers_vehicles (vehicle_no, id_customer) values (@vehicle_no, @id_customer)",
                new
                {
                    vehicle_no = customerVehicle.Number,
                    id_customer = customerId
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customerVehicle.CustomerId = customerId;
        }

        /// <summary>
        ///     Updates last visited date of existing customer vehicle in database
        /// </summary>
        /// <param name="customerVehicle"></param>
        internal void UpdateCustomerVehicleDate(CustomerVehicle customerVehicle)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update customers_vehicles set date_last_seen = CURRENT_TIMESTAMP where vehicle_no = @vehicle_no",
                new
                {
                    vehicle_no = customerVehicle.Number
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of customer vehicles matching the customer's Id
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        internal IEnumerable<CustomerVehicle> GetCustomerVehicles(Customer customer)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select vehicle_no 'Number', id_customer 'OrderId', date_last_seen 'LastVisitDate' from customers_vehicles " +
                "where id_customer = @id_customer " +
                "order by vehicle_no",
                new
                {
                    id_customer = customer.Id
                });

            // Execute sql command
            return Connection.Query<CustomerVehicle>(command);
        }
    }
}