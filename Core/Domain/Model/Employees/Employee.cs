using System;
using System.Linq;
using System.Collections.Generic;
using Core.Domain.Enums;

namespace Core.Domain.Model.Employees
{
    public class Employee : Person
    {
        public Employee(string name, string contact, bool isActive, AccessMode accessMode, IEnumerable<EmployeePayment> employeePaymets = null) : base(name, contact)
        {
            IsActive = isActive;
            AccessMode = accessMode;
            EmployeePayments = employeePaymets?.ToList();
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Employee()
        {
            AccessMode = AccessMode.None;
            EmployeePayments =null;
        }
        
        public DateTime? LastPaidDate { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public AccessMode AccessMode { get; set; }
        public List<EmployeePayment> EmployeePayments { get; set; }
    }
}