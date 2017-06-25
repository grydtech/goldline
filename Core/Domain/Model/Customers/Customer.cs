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
            var maxIndex = Nic.Length - 1;
            return int.TryParse(Nic.Substring(1, maxIndex - 1), out int n) &&
                   (Nic.Substring(maxIndex).ToUpper() == "V" || Nic.Substring(maxIndex).ToUpper() == "X");
        }
    }
}