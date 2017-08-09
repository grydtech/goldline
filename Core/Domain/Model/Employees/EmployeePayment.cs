using System;

namespace Core.Domain.Model.Employees
{
    public class EmployeePayment : Payment
    {
        public EmployeePayment(uint employeeId, decimal amount, string reason) : base(DateTime.Now, amount)
        {
            EmployeeId = employeeId;
            Reason = reason;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public EmployeePayment()
        {
        }

        public uint EmployeeId { get; set; }
        public string Reason { get; set; }
    }
}