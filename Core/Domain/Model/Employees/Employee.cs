using System;
using Core.Domain.Enums;

namespace Core.Domain.Model.Employees
{
    public class Employee : Person
    {
        public Employee(string name, string contact, bool isActive, AccessMode accessMode) : base(name, contact)
        {
            IsActive = isActive;
            AccessMode = accessMode;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public Employee()
        {
            AccessMode = AccessMode.None;
        }

        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public AccessMode AccessMode { get; set; }
    }
}