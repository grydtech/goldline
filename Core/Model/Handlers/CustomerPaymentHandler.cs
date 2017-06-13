using System;
using System.Collections.Generic;
using Core.Datalayer;
using Core.Model.Payments;
using Core.Model.Persons;
using Core.Security;

namespace Core.Model.Handlers
{
    public class CustomerPaymentHandler
    {
        /// <summary>
        ///     Adds a new payment to customer and return if successful or not
        /// </summary>
        /// <param name="customerPayment"></param>
        public void AddNewCustomerPayment(CustomerPayment customerPayment)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new CustomerPaymentDal(connection).InsertCustomerPayment(customerPayment, User.CurrentUser.EmployeeId);
            }
        }

        /// <summary>
        ///     Undo an erroneous customer payment
        /// </summary>
        /// <param name="customerPayment"></param>
        public void UndoCustomerPayment(CustomerPayment customerPayment)
        {
            // Exception handling
            if (customerPayment.Id == null)
                throw new ArgumentNullException(nameof(customerPayment.Id),
                    "Attempted removing customerpayment with Id null");

            using (var connection = ConnectionManager.GetConnection())
            {
                new CustomerPaymentDal(connection).RemoveCustomerPayment((uint) customerPayment.Id);
            }
        }

        /// <summary>
        ///     Gets a list of payments done by a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isLimited">By default only returns 10 records. If more needed, mark this as false</param>
        /// <returns></returns>
        public IEnumerable<CustomerPayment> GetCustomerPayments(Customer customer, bool isLimited = true)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                return new CustomerPaymentDal(connection).GetCustomerPayments((uint) customer.Id,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }

        /// <summary>
        ///     Gets the most recent payments by each customer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerPayment> GetMostRecentPayments()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new CustomerPaymentDal(connection).GetRecentPayments(Constraints.DefaultLimit);
            }
        }
    }
}