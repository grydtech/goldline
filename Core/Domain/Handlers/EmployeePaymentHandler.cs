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
        /// <param name="payment"></param>
        public void AddPayment(EmployeePayment payment)
        {
            using (var connection = Connector.GetConnection())
            {
                var employeePaymentDal = new EmployeePaymentDal(connection);
                employeePaymentDal.Insert(payment.EmployeeId, payment.Amount, payment.Note);
                payment.Id = employeePaymentDal.GetLastInsertId();
            }
        }

        /// <summary>
        ///     Undo an erroneous employee payment
        /// </summary>
        /// <param name="payment"></param>
        public void DeletePayment(EmployeePayment payment)
        {
            // Exception handling
            if (payment.Id == null)
                throw new ArgumentNullException(nameof(payment.Id), "Employee payment id is null");
            using (var connection = Connector.GetConnection())
            {
                new EmployeePaymentDal(connection).Delete(payment.Id.Value);
            }
        }

        /// <summary>
        ///     Gets a list of payments done to an employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public IEnumerable<EmployeePayment> GetPayments(Employee employee = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new EmployeePaymentDal(connection).Search(employee?.Id);
            }
        }
    }
}