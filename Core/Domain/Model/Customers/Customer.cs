namespace Core.Domain.Model.Customers
{
    public class Customer : Person
    {
        public Customer(string name, string contact, string nic) : base(name, contact)
        {
            Nic = nic;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Customer()
        {
        }

        public string Nic { get; set; }
        public decimal DueAmount { get; set; }
    }
}