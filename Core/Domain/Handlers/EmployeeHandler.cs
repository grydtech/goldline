using System;
using System.Collections.Generic;
using Core.Data;
using System.Linq;
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
        public IEnumerable<Employee> GetEmployees(string name = null, bool? isActive = null, bool isLastPaymentDateLoaded = false)
        {
            using (var connection = Connector.GetConnection())
            {
                return new EmployeeDal(connection).Search(name == null ? null : $"%{name}%", isActive,isLastPaymentDateLoaded:isLastPaymentDateLoaded);
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
        /// <summary>
        ///     Load Employee Paymnts of a employee
        /// </summary>
        /// <param name="employee"></param>
        public void LoadEmployeePayments(Employee employee)
        {
            if (employee.Id == null)
                throw new ArgumentNullException(nameof(employee.Id), "Employee Id is null");

            var employeePaymentHandler = new EmployeePaymentHandler();

            using (var connection = Connector.GetConnection())
            {//Search(uint? employeeId = null, int offset = 0, int limit = int.MaxValue)
                var employeePayemnts= new EmployeePaymentDal(connection).Search(employee.Id.Value,limit:5)?.ToList();
                employee.EmployeePayments = employeePayemnts ?? throw new NullReferenceException("employee payements empty");
            }
        }
    }
}