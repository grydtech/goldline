using System;
using System.Collections.Generic;
using Core.Data;
using Core.Model.Payments;
using Core.Model.Persons;
using Core.Security;

namespace Core.Model.Handlers
{
    public class EmployeePaymentHandler
    {
        /// <summary>
        ///     Adds a new payment to employee
        /// </summary>
        /// <param name="employeePayment"></param>
        public void AddNewEmployeePayment(EmployeePayment employeePayment)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new EmployeePaymentDal(connection).InsertEmployeePayment(employeePayment, User.CurrentUser.EmployeeId);
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
            using (var connection = ConnectionManager.GetConnection())
            {
                new EmployeePaymentDal(connection).RemoveEmployeePayment((uint) employeePayment.Id);
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

            using (var connection = ConnectionManager.GetConnection())
            {
                return new EmployeePaymentDal(connection).GetEmployeePayments((uint) employee.Id,
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
            using (var connection = ConnectionManager.GetConnection())
            {
                return new EmployeePaymentDal(connection).GetRecentEmployeePayments(
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }
    }
}