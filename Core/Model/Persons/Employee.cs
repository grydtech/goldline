using System;
using Core.Model.Enums;

namespace Core.Model.Persons
{
    public class Employee : Person
    {
        public Employee(string name, string contact, EmployeeType employeeType) : base(name, contact)
        {
            IsActive = true;
            EmployeeType = employeeType;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Employee()
        {
        }

        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastPaymentDate { get; set; }

        public EmployeeType EmployeeType { get; set; }
    }
}