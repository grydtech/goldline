using System;

namespace Core.Domain.Model.Employees
{
    public class EmployeePayment : Payment
    {
        public EmployeePayment(uint employeeId, decimal amount, string note) : base(DateTime.Now, amount)
        {
            EmployeeId = employeeId;
            Note = note;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public EmployeePayment()
        {
        }

        public uint EmployeeId { get; set; }
        public string Note { get; set; }
    }
}