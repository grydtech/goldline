using System.Collections.Generic;
using System.Data;
using Core.Model;
using Core.Model.Persons;
using Dapper;

namespace Core.Data
{
    internal class CustomersDal : Dal
    {
        internal CustomersDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
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
                "insert into customers (name_customer, nic, contact, due_amount) values (@name_customer, @nic, @contact, @due_amount)",
                new
                {
                    name_customer = customer.Name,
                    nic = customer.Nic,
                    contact = customer.Contact,
                    due_amount = customer.DueAmount
                }, 
                Transaction);

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            customer.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Updates a customer in the database.
        ///     The properties that will be updated are: Name, Nic, Contact, DueAmount
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="nic"></param>
        /// <param name="contact"></param>
        /// <param name="dueAmount"></param>
        internal void UpdateCustomer(uint customerId, string name, string nic, string contact, string dueAmount)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update customers set name_customer = @name, nic = @nic, contact = @contact, due_amount = @dueAmount " +
                "where id_customer = @customerId",
                new {customerId, name, nic, contact, dueAmount}, 
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of customers matching the search parameters
        /// </summary>
        /// <param name="name">search by name</param>
        /// <returns></returns>
        internal IEnumerable<Customer> FindCustomersByName(string name)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_customer 'Id', name_customer 'Name', nic 'Nic', contact 'Contact', due_amount 'DueAmount' from customers " +
                "where name_customer like @name_customer " +
                "order by name_customer",
                new{name_customer = "%" + name + "%"},
                Transaction);

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
                "select id_customer 'Id', name_customer 'Name', nic 'Nic', contact 'Contact', due_amount 'DueAmount' from customers " +
                "order by name_customer",
                null,
                Transaction);

            // Execute sql command
            return Connection.Query<Customer>(command);
        }
    }
}