namespace Core.Model.Payments
{
    public class EmployeePayment : Payment
    {
        public EmployeePayment(decimal amount, string note, uint employeeId) : base(amount, note)
        {
            EmployeeId = employeeId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public EmployeePayment()
        {
        }

        public uint EmployeeId { get; set; }
    }
}