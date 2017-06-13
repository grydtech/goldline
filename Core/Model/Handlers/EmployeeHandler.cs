using System;
using System.Collections.Generic;
using Core.Datalayer;
using Core.Model.Persons;

namespace Core.Model.Handlers
{
    public class EmployeeHandler
    {
        /// <summary>
        ///     Adds a new employee and return if successful
        /// </summary>
        /// <param name="employee"></param>
        public void AddNewEmployee(Employee employee)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new EmployeeDal(connection).InsertEmployee(employee);
            }
        }

        /// <summary>
        ///     Returns a list of employees matching the given search criteria
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public IEnumerable<Employee> SearchEmployee(string name, bool? isActive = null)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new EmployeeDal(connection).GetEmployees(name, isActive);
            }
        }

        /// <summary>
        ///     Update employee with new Name and Contact properties
        /// </summary>
        /// <param name="employee"></param>
        public void UpdateEmployeeDetails(Employee employee)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new EmployeeDal(connection).UpdateEmployeeDetails(employee);
            }
        }

        /// <summary>
        ///     Returns all employees
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> GetAllEmployees()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new EmployeeDal(connection).GetAllEmployees();
            }
        }

        /// <summary>
        ///     Mark employee as active or inactive
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public void UpdateEmployeeStatus(Employee employee, bool isActive)
        {
            // Exception handling
            if (employee.Id == null) throw new ArgumentNullException(nameof(employee.Id), "Employee id is null");
            using (var connection = ConnectionManager.GetConnection())
            {
                new EmployeeDal(connection).UpdateEmployeeStatus((uint) employee.Id, isActive);
            }
        }
    }
}