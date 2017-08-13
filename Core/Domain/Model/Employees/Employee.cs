using System;
using System.Linq;
using System.Collections.Generic;
using Core.Domain.Enums;

namespace Core.Domain.Model.Employees
{
    public class Employee : Person
    {
        public Employee(string name, string contact, bool isActive, AccessMode accessMode, IEnumerable<EmployeePayment> orderEntries=null) : base(name, contact)
        {
            IsActive = isActive;
            AccessMode = accessMode;
            EmployeePayments = orderEntries?.ToList() ?? new List<EmployeePayment>();
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Employee()
        {
            AccessMode = AccessMode.None;
            EmployeePayments = new List<EmployeePayment>();
        }
        
        public DateTime? LastPaidDate { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public AccessMode AccessMode { get; set; }
        public List<EmployeePayment> EmployeePayments { get; set; }
    }
}