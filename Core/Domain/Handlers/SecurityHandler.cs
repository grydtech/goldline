using System;
using System.Transactions;
using Core.Data;
using Core.Data.Employees;
using Core.Domain.Enums;
using Core.Domain.Model.Employees;

namespace Core.Domain.Handlers
{
    public class SecurityHandler
    {
        /// <summary>
        ///     Try logging in with the provided credentials or with default password
        ///     and return User object if successfully authenticated, else null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User TryAuthentication(string username, string password)
        {
            using (var connection = Connector.GetConnection())
            {
                var userAccessDal = new UserDal(connection);
                return userAccessDal.Search(username, password) ??
                       userAccessDal.Search(username, User.DefaultPassword);
            }
        }

        /// <summary>
        ///     Changes the current password of a user and assigns new password to it if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public void ChangePassword(User user, string newPassword)
        {
            using (var connection = Connector.GetConnection())
            {
                new UserDal(connection).UpdateUserPassword(user, newPassword);
            }
        }

        /// <summary>
        ///     Provides an employee with user access credentials and return user object
        ///     The user inserted will have its password set as the default password,
        ///     and the employee will have its usertype set to User
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="userType"></param>
        /// <param name="username"></param>
        public User AddNewUserAccess(Employee employee, AccessMode userType, string username)
        {
            // Exception handling
            if (employee.AccessMode == EmployeeType.User)
                throw new ArgumentException("AccessMode of employee is already User", nameof(employee.AccessMode));
            if (employee.Id == null)
                throw new ArgumentNullException(nameof(employee.Id), "Employee Id is null");

            User newUser;
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    new EmployeeDal(connection).UpdateEmployeeType(employee, EmployeeType.User);
                    newUser = new User((uint) employee.Id, userType, username);
                    new UserDal(connection).Insert(newUser);
                }
                scope.Complete();
            }

            // Returns user if successful
            return newUser;
        }

        /// <summary>
        ///     Elevate or lower user access privileges to user and return if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newUserType"></param>
        public void UpdateUserAccess(User user, AccessMode newUserType)
        {
            using (var connection = Connector.GetConnection())
            {
                new UserDal(connection).Update(user, newUserType);
            }
        }

        /// <summary>
        ///     Returns the user object if available, for given employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public User GetUser(uint employeeId)
        {
            using (var connection = Connector.GetConnection())
            {
                return new UserDal(connection).Search(employeeId);
            }
        }

        /// <summary>
        ///     Check if username is available
        /// </summary>
        public bool IsUsernameAvailable(string username)
        {
            using (var connection = Connector.GetConnection())
            {
                return new UserDal(connection).IsUsernameAvailable(username);
            }
        }
    }
}