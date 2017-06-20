using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
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
    }
}