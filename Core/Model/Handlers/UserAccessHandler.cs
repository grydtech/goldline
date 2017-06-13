using System;
using System.Transactions;
using Core.Datalayer;
using Core.Model.Enums;
using Core.Model.Persons;
using Core.Security;

namespace Core.Model.Handlers
{
    public class UserAccessHandler
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
            using (var connection = ConnectionManager.GetConnection())
            {
                var userAccessDal = new UserAccessDal(connection);
                return userAccessDal.GetUser(username, password) ??
                       userAccessDal.GetUser(username, User.DefaultPassword);
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
            using (var connection = ConnectionManager.GetConnection())
            {
                new UserAccessDal(connection).UpdateUserPassword(user, newPassword);
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
        public User AddNewUserAccess(Employee employee, UserType userType, string username)
        {
            // Exception handling
            if (employee.EmployeeType == EmployeeType.User)
                throw new ArgumentException("EmployeeType of employee is already User", nameof(employee.EmployeeType));
            if (employee.Id == null)
                throw new ArgumentNullException(nameof(employee.Id), "Employee Id is null");

            User newUser;
            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    new EmployeeDal(connection).UpdateEmployeeType(employee, EmployeeType.User);
                    newUser = new User((uint) employee.Id, userType, username);
                    new UserAccessDal(connection).InsertUser(newUser);
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
        public void UpdateUserAccess(User user, UserType newUserType)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new UserAccessDal(connection).UpdateUserType(user, newUserType);
            }
        }

        /// <summary>
        ///     Returns the user object if available, for given employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public User GetUser(uint employeeId)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new UserAccessDal(connection).GetUser(employeeId);
            }
        }

        /// <summary>
        ///     Check if username is available
        /// </summary>
        public bool IsUsernameAvailable(string username)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new UserAccessDal(connection).IsUsernameAvailable(username);
            }
        }
    }
}