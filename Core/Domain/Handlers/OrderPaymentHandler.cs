using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Customers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Employees;

namespace Core.Domain.Handlers
{
    public class OrderPaymentHandler
    {
        /// <summary>
        ///     Adds a new payment to customer and return if successful or not
        /// </summary>
        /// <param name="orderPayment"></param>
        public void AddNewCustomerPayment(OrderPayment orderPayment)
        {
            using (var connection = Connector.GetConnection())
            {
                new OrderPaymentDal(connection).Insert(TODO, TODO);
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
                new OrderPaymentDal(connection).Delete((uint) orderPayment.Id);
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
                return new OrderPaymentDal(connection).Search(customer.Id.Value);
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
                return new OrderPaymentDal(connection).SearchRecent(Constraints.DefaultLimit);
            }
        }
    }
}