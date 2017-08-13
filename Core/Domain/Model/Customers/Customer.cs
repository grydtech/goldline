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

        /// <summary>
        ///     Method to check if entered nic has correct format
        /// </summary>
        /// <returns></returns>
        public bool IsNicValid()
        {
            if (Nic.Length != 10) return false;
            return int.TryParse(Nic.Substring(0, 9), out int _) &&
                   (Nic.Substring(9).ToUpper() == "V" || Nic.Substring(9).ToUpper() == "X");
        }
    }
}