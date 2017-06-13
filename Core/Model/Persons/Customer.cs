namespace Core.Model.Persons
{
    public class Customer : Person
    {
        public Customer(string name, string contact, string nic, decimal dueAmount = 0) : base(name, contact)
        {
            Nic = nic;
            DueAmount = dueAmount;
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