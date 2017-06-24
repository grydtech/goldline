using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Employees;
using Core.Domain.Model.Employees;

namespace Core.Domain.Handlers
{
    public class EmployeePaymentHandler
    {
        /// <summary>
        ///     Adds a new payment to employee
        /// </summary>
        /// <param name="employeePayment"></param>
        public void AddNewEmployeePayment(EmployeePayment employeePayment)
        {
            using (var connection = Connector.GetConnection())
            {
                new EmployeePaymentDal(connection).Insert(employeePayment, User.CurrentUser.EmployeeId);
            }
        }

        /// <summary>
        ///     Undo an erroneous employee payment
        /// </summary>
        /// <param name="employeePayment"></param>
        public void UndoEmployeePayment(EmployeePayment employeePayment)
        {
            // Exception handling
            if (employeePayment.Id == null)
                throw new ArgumentNullException(nameof(employeePayment.Id), "Employee payment id is null");
            using (var connection = Connector.GetConnection())
            {
                new EmployeePaymentDal(connection).Delete((uint) employeePayment.Id);
            }
        }

        /// <summary>
        ///     Gets a list of payments done to an employee
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="isLimited">Select whether the umber of records loaded is limited</param>
        /// <returns></returns>
        public IEnumerable<EmployeePayment> GetEmployeePayments(Employee employee, bool isLimited = true)
        {
            // Exception handling
            if (employee.Id == null) throw new ArgumentNullException(nameof(employee.Id), "Employee id is null");

            using (var connection = Connector.GetConnection())
            {
                return new EmployeePaymentDal(connection).Search((uint) employee.Id,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }

        /// <summary>
        ///     Gets a list of most recent employee payments
        /// </summary>
        /// <param name="isLimited">Select whether the number of records loaded is limited</param>
        /// <returns></returns>
        public IEnumerable<EmployeePayment> GetRecentEmployeePayments(bool isLimited = true)
        {
            using (var connection = Connector.GetConnection())
            {
                return new EmployeePaymentDal(connection).GetRecentEmployeePayments(
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }
    }
}