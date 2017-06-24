using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Employees;
using Dapper;

namespace Core.Data.Employees
{
    internal class EmployeeDal : Dal
    {
        internal EmployeeDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [employees] table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        internal void Insert(string name, string contact)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into employees (name, contact) values (@name, @contact)",
                new {name, contact});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [employees] table
        /// </summary>
        /// <param name="nameExp"></param>
        /// <param name="isActive"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<Employee> Search(string nameExp = null, bool? isActive = null, int offset = 0,
            int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'Id', name 'Name', (SELECT COALESCE(type_user-0, 0) from users U WHERE U.id_employee = E.id_employee) 'AccessMode', contact 'Contact', date_joined 'DateJoined', is_active 'IsActive' " +
                "from employees E " +
                (nameExp == null && isActive == null ? "" : "where ") +
                (nameExp == null ? "" : "name LIKE @nameExp ") +
                (isActive == null ? "" : (nameExp == null ? "" : "and ") + "is_active = @isActive ") +
                "order by name_employee limit @offset, @limit",
                new {nameExp, isActive, offset, limit});

            // Execute sql command
            return Connection.Query<Employee>(command);
        }

        /// <summary>
        ///     Updates a record in [employees] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="contact"></param>
        /// <param name="isActive"></param>
        /// <param name="name"></param>
        internal void Update(uint employeeId, string name = null, string contact = null, bool? isActive = null)
        {
            if (name == null && contact == null && isActive == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update employees set " +
                ((name == null ? "" : "name = @name, ") +
                 (contact == null ? "" : "contact = @contact, ") +
                 (isActive == null ? "" : "is_active = @isActive, ")).TrimEnd(' ', ',') +
                " where id_employee = @employeeId",
                new {employeeId, name, contact, isActive});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}