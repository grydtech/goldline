using System;
using System.Collections.Generic;
using Core.Data;
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
        /// <param name="orderPayment"></param>
        public void AddNewCustomerPayment(OrderPayment orderPayment)
        {
            using (var connection = Connector.GetConnection())
            {
                new OrderPaymentDal(connection).InsertOrderPayment(orderPayment, User.CurrentUser.EmployeeId);
            }
        }

        /// <summary>
        ///     Undo an erroneous customer payment
        /// </summary>
        /// <param name="orderPayment"></param>
        public void UndoCustomerPayment(OrderPayment orderPayment)
        {
            // Exception handling
            if (orderPayment.Id == null)
                throw new ArgumentNullException(nameof(orderPayment.Id),
                    "Attempted removing customerpayment with Id null");

            using (var connection = Connector.GetConnection())
            {
                new OrderPaymentDal(connection).RemoveOrderPayment((uint) orderPayment.Id);
            }
        }

        /// <summary>
        ///     Gets a list of payments done by a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isLimited">By default only returns 10 records. If more needed, mark this as false</param>
        /// <returns></returns>
        public IEnumerable<OrderPayment> GetCustomerPayments(Customer customer, bool isLimited = true)
        {
            // Exception handling
            if (customer.Id == null) throw new ArgumentNullException(nameof(customer.Id), "Customer Id is null");

            using (var connection = Connector.GetConnection())
            {
                return new OrderPaymentDal(connection).GetOrderPayments((uint) customer.Id,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }

        /// <summary>
        ///     Gets the most recent payments by each customer
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderPayment> GetMostRecentPayments()
        {
            using (var connection = Connector.GetConnection())
            {
                return new OrderPaymentDal(connection).GetRecentPayments(Constraints.DefaultLimit);
            }
        }
    }
}