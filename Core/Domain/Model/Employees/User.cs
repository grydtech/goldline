using Core.Domain.Enums;
using Core.Domain.Security;

namespace Core.Domain.Model.Employees
{
    public class User
    {
        public User(uint employeeId, AccessMode accessMode, string username, string password = Session.DefaultPassword)
        {
            EmployeeId = employeeId;
            Username = username;
            Password = password;
            AccessMode = accessMode;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public User()
        {
        }

        public uint EmployeeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccessMode AccessMode { get; set; } = AccessMode.None;
        public Clearance Clearance => new Clearance(AccessMode);

        /// <summary>
        ///     Returns whether the current user password is the default password
        /// </summary>
        /// <returns></returns>
        public bool IsDefaultPassword()
        {
            return Password == Session.DefaultPassword;
        }
    }
}