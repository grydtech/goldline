using System;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Customers;
using Core.Domain.Model.Customers;

namespace Core.Domain.Handlers
{
    public class OrderPaymentHandler
    {
        /// <summary>
        ///     Adds a new payment to customer and return if successful or not
        /// </summary>
        /// <param name="payment"></param>
        public void AddPayment(OrderPayment payment)
        {
            using (var connection = Connector.GetConnection())
            {
                var paymentDal = new OrderPaymentDal(connection);
                paymentDal.Insert(payment.OrderId, payment.Amount);
                payment.Id = paymentDal.GetLastInsertId();
            }
        }

        /// <summary>
        ///     Undo an erroneous customer payment
        /// </summary>
        /// <param name="orderPayment"></param>
        public void DeletePayment(OrderPayment orderPayment)
        {
            // Exception handling
            if (orderPayment.Id == null)
                throw new ArgumentNullException(nameof(orderPayment.Id),
                    "Attempted removing customerpayment with Id null");

            using (var connection = Connector.GetConnection())
            {
                new OrderPaymentDal(connection).Delete(orderPayment.Id.Value);
            }
        }

        /// <summary>
        ///     Gets a list of payments
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<OrderPayment> GetPayments(uint? customerId = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new OrderPaymentDal(connection).Search(customerId);
            }
        }
    }
}