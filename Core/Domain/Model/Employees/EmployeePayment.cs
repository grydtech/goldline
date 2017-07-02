using System;

namespace Core.Domain.Model.Employees
{
    public class EmployeePayment : Payment
    {
        public EmployeePayment(uint employeeId, decimal amount, string note) : base(DateTime.Now, amount, note)
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