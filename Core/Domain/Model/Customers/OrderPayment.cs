namespace Core.Domain.Model.Customers
{
    public class OrderPayment : Payment
    {
        public OrderPayment(uint orderId, decimal amount, string note) : base(amount, note)
        {
            OrderId = orderId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public OrderPayment()
        {
        }

        public uint OrderId { get; set; }
    }
}