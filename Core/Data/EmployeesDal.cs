using System.Collections.Generic;
using System.Data;
using Core.Model.Enums;
using Core.Model.Persons;
using Dapper;
using static Core.Data.SqlGenerator;

namespace Core.Data
{
    internal class EmployeesDal : Dal
    {
        internal EmployeesDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Adds a new employee record to database and assigns the Id
        /// </summary>
        /// <param name="employee"></param>
        internal void InsertEmployee(Employee employee)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into employees (name, contact) values (@name, @contact)",
                new
                {
                    name = employee.Name,
                    contact = employee.Contact
                }, Transaction);

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            employee.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Returns a list of employees matching given search parameters
        /// </summary>
        /// <param name="name">search by name</param>
        /// <param name="isActive">if null, return both active and inactive employees, else return only given type</param>
        internal IEnumerable<Employee> GetEmployees(string name, bool? isActive = null)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'Id', name 'Name', contact 'Contact', date_joined 'DateJoined', is_active 'IsActive' " +
                $"from employees where name like @name_employee {(isActive != null ? "and is_active = @isActive" : "")} " +
                "order by name",
                new
                {
                    name_employee = "%" + name + "%",
                    isActive
                }, Transaction);

            // Execute sql command
            return Connection.Query<Employee>(command);
        }

        /// <summary>
        ///     Returns a list of all employees
        /// </summary>
        /// <param name="isActive">if null, return both active and inactive, else return only given type</param>
        internal IEnumerable<Employee> GetAllEmployees(bool? isActive = null)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'Id', name 'Name', contact 'Contact', date_joined 'DateJoined', is_active 'IsActive' " +
                $"from employees {(isActive != null ? "where is_active = @isActive" : "")} " +
                "order by name",
                new
                {
                    isActive
                }, Transaction);

            // Execute sql command
            return Connection.Query<Employee>(command);
        }

        /// <summary>
        ///     Updates an existing employee personal details.
        ///     If property is passed then it will be updated.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        /// <param name="isActive"></param>
        internal void UpdateEmployeeDetails(uint employeeId, string name = null, string contact = null, bool? isActive = null)
        {
            if (name == null && contact == null) return;
            // Define sql command
            var command = new CommandDefinition(
                Update("employees").SetIfNotNull(new {name, contact, is_active = isActive}).Where(new {id_employee = employeeId}),
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }
    }
}