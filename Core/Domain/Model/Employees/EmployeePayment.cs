using System;

namespace Core.Domain.Model.Employees
{
    public class EmployeePayment : Payment
    {
        public EmployeePayment(DateTime date, decimal amount, string note, uint employeeId) : base(date, amount, note)
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