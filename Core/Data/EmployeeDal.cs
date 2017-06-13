using System.Collections.Generic;
using System.Data;
using Core.Model.Enums;
using Core.Model.Persons;
using Dapper;

namespace Core.Data
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
                "insert into employees (name_employee, type_employee, contact) values (@name_employee, @type_employee, @contact)",
                new
                {
                    name_employee = employee.Name,
                    type_employee = employee.EmployeeType.ToString(),
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
                "select id_employee 'Id', name_employee 'Name', type_employee-1 'EmployeeType', contact 'Contact', date_joined 'DateJoined', " +
                "date_last_payment 'LastPaymentDate', is_active 'IsActive' " +
                "from employees where name_employee like @name_employee " +
                "order by name_employee",
                new
                {
                    name_employee = "%" + name + "%"
                });

            var commandActive = new CommandDefinition(
                "select id_employee 'Id', name_employee 'Name', type_employee-1 'EmployeeType', contact 'Contact', date_joined 'DateJoined', " +
                "date_last_payment 'LastPaymentDate', is_active 'IsActive' " +
                "from employees where name_employee like @name_employee and is_active = @is_active " +
                "order by name_employee",
                new
                {
                    name_employee = "%" + name + "%",
                    is_active = isActive
                });

            // Execute sql command
            return Connection.Query<Employee>(isActive == null ? command : commandActive);
        }

        /// <summary>
        ///     Returns a list of all employees
        /// </summary>
        /// <param name="isActive">if null, return both active and inactive, else return only given type</param>
        internal IEnumerable<Employee> GetAllEmployees(bool? isActive = null)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'Id', name_employee 'Name', type_employee-1 'EmployeeType', contact 'Contact', date_joined 'DateJoined', " +
                "date_last_payment 'LastPaymentDate', is_active 'IsActive' " +
                "from employees " +
                "order by name_employee");

            var commandActive = new CommandDefinition(
                "select id_employee 'Id', name_employee 'Name', type_employee-1 'EmployeeType', contact 'Contact', date_joined 'DateJoined', " +
                "date_last_payment 'LastPaymentDate', is_active 'IsActive' " +
                "from employees where is_active = @is_active " +
                "order by name_employee",
                new
                {
                    is_active = isActive
                });

            // Execute sql command
            return Connection.Query<Employee>(isActive == null ? command : commandActive);
        }

        /// <summary>
        ///     Updates an existing employee personal details.
        ///     The properties that will be updated are : Name, Contact
        /// </summary>
        /// <param name="employee"></param>
        internal void UpdateEmployeeDetails(Employee employee)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update employees set name_employee = @name_employee, contact = @contact " +
                "where id_employee = @id_employee",
                new
                {
                    id_employee = employee.Id,
                    name_employee = employee.Name,
                    contact = employee.Contact
                });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates existing employee with new employee type and assign it to employee
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeType"></param>
        internal void UpdateEmployeeType(Employee employee, EmployeeType employeeType)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update employees set type_employee = @type_employee where id_employee = @id_employee",
                new
                {
                    id_employee = employee.Id,
                    type_employee = employeeType.ToString()
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            employee.EmployeeType = employeeType;
        }

        /// <summary>
        ///     Updates whether employee is active or not
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="isActive"></param>
        internal void UpdateEmployeeStatus(uint employeeId, bool isActive)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update employees set is_active = @is_active where id_employee = @id_employee",
                new
                {
                    id_employee = employeeId,
                    is_active = isActive
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}