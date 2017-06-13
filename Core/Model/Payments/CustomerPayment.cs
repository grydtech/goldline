namespace Core.Model.Payments
{
    public class CustomerPayment : Payment
    {
        public CustomerPayment(decimal amount, string note, uint customerId) : base(amount, note)
        {
            CustomerId = customerId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public CustomerPayment()
        {
        }

        public uint CustomerId { get; set; }
    }
}