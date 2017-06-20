namespace Core.Model.Payments
{
    public class OrderPayment : Payment
    {
        public OrderPayment(decimal amount, string note, uint orderId) : base(amount, note)
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