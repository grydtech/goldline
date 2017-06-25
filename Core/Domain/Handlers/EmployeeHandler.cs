using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Employees;
using Core.Domain.Model.Employees;

namespace Core.Domain.Handlers
{
    public class EmployeeHandler
    {
        /// <summary>
        ///     Adds a new employee and return if successful
        /// </summary>
        /// <param name="employee"></param>
        public void AddEmployee(Employee employee)
        {
            // Exception handling
            if (employee == null) throw new ArgumentNullException(nameof(employee), "Employee is null");
            if (employee.Name == null) throw new ArgumentNullException(nameof(employee.Name), "Employee name is null");
            if (employee.Contact == null)
                throw new ArgumentNullException(nameof(employee.Contact), "Employee contact is null");

            using (var connection = Connector.GetConnection())
            {
                var employeeDal = new EmployeeDal(connection);
                employeeDal.Insert(employee.Name, employee.Contact);
                employee.Id = employeeDal.GetLastInsertId();
            }
        }

        /// <summary>
        ///     Returns a list of employees matching the given search criteria
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetEmployees(string name = null, bool? isActive = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new EmployeeDal(connection).Search(name == null ? null : $"%{name}%", isActive);
            }
        }

        /// <summary>
        ///     Update employee with new Name and Contact properties
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        /// <param name="isActive"></param>
        public void UpdateEmployee(Employee employee, string name = null, string contact = null, bool? isActive = null)
        {
            // Exception handling
            if (employee == null) throw new ArgumentNullException(nameof(employee), "Employee is null");
            if (employee.Id == null) throw new ArgumentNullException(nameof(employee.Id), "Employee Id is null");

            using (var connection = Connector.GetConnection())
            {
                new EmployeeDal(connection).Update(employee.Id.Value, name, contact, isActive);
                employee.Name = name ?? employee.Name;
                employee.Contact = contact ?? employee.Contact;
                employee.IsActive = isActive ?? employee.IsActive;
            }
        }
    }
}