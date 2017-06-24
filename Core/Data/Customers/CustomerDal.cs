using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class CustomerDal : Dal
    {
        private const string DueAmtExp = "(SELECT COALESCE(SUM(due_amount),0) from orders_view O where O.id_customer = C.id_customer)";
        internal CustomerDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [customers] table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nic"></param>
        /// <param name="contact"></param>
        internal void Insert(string name, string nic, string contact)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers (name, nic, contact) values (@name, @nic, @contact)",
                new {name, nic, contact});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [customers] table
        /// </summary>
        /// <param name="nameExp">search by name</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>Customer objects</returns>
        internal IEnumerable<Customer> Search(string nameExp = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_customer 'Id', name 'Name', nic 'Nic', contact 'Contact', " +
                $"{DueAmtExp} 'DueAmount' from customers C " +
                (nameExp == null? "" : "where name like @nameExp ") +
                "order by name limit @offset, @limit",
                new {nameExp, offset, limit});

            // Execute sql command
            return Connection.Query<Customer>(command);
        }

        /// <summary>
        ///     Updates a record in [customers] table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="nic"></param>
        /// <param name="contact"></param>
        internal void Update(uint id, string name = null, string nic = null, string contact = null)
        {
            if (name == null && nic == null && contact == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");
            // Define sql command
            var command = new CommandDefinition(
                "update customers set " +
                ((name == null ? "" : "name = @name, ") +
                (nic == null ? "" : "nic = @nic, ") +
                (contact == null ? "" : "contact = @contact, ")).TrimEnd(' ', ',') +
                " where id_customer = @id",
                new {id, name, nic, contact});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [customers] table
        /// </summary>
        /// <param name="id"></param>
        internal void Delete(uint id)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from customers where id_customer = @id",
                new { id });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}