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
        ///     Adds a new employee record to database and assigns the Id
        /// </summary>
        /// <param name="employee"></param>
        internal void InsertEmployee(Employee employee)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into employees (name, contact) values (@name_employee, @contact)",
                new
                {
                    name = employee.Name,
                    contact = employee.Contact
                });

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
        internal IEnumerable<Employee> GetEmployees(string name, bool? isActive)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'Id', name 'Name', employee_type(id_employee) 'AccessMode', contact 'Contact', date_joined 'DateJoined', is_active 'IsActive' " +
                "from employees where name like @name " +
                (isActive == null ? "" : "and is_active = @isActive ") +
                "order by name_employee",
                new
                {
                    name = "%" + name + "%",
                    isActive
                });

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
                "select id_employee 'Id', name_employee 'Name', type_employee-1 'AccessMode', contact 'Contact', date_joined 'DateJoined', " +
                "date_last_payment 'LastPaymentDate', is_active 'IsActive' " +
                "from employees " +
                (isActive == null ? "" : "where is_active = @isActive ") +
                "order by name_employee",
                new
                {
                    isActive
                });

            // Execute sql command
            return Connection.Query<Employee>(command);
        }

        /// <summary>
        ///     Updates an existing employee personal details.
        ///     The properties that will be updated are : Name, Contact
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="isActive"></param>
        internal void UpdateEmployeeDetails(Employee employee, bool? isActive)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update employees set name = @name, contact = @contact " +
                (isActive == null ? "" : ", is_active = @isActive ") +
                "where id_employee = @id_employee",
                new
                {
                    id_employee = employee.Id,
                    isActive,
                    name = employee.Name,
                    contact = employee.Contact
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}