using System;
using System.Linq;
using System.Transactions;
using Core.Data;
using Core.Data.Employees;
using Core.Domain.Enums;
using Core.Domain.Model;
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
                var userDal = new UserDal(connection);
                var result = userDal.Search(username: username, password: password) ??
                             userDal.Search(username: username, password: Session.DefaultPassword);
                return result.SingleOrDefault();
            }
        }

        /// <summary>
        ///     Changes the current password of a user and assigns new password to it if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void ChangePassword(User user, string password)
        {
            using (var connection = Connector.GetConnection())
            {
                new UserDal(connection).Update(user.EmployeeId, password: password);
            }
        }

        /// <summary>
        ///     Provides an employee with user access credentials and return user object
        ///     The user inserted will have its password set as the default password,
        ///     and the employee will have its usertype set to User
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="accessMode"></param>
        /// <param name="username"></param>
        public User AddUserAccess(Employee employee, AccessMode accessMode, string username)
        {
            // Exception handling
            if (employee.Id == null)
                throw new ArgumentNullException(nameof(employee.Id), "Employee Id is null");
            if (employee.AccessMode != AccessMode.None)
                throw new ArgumentException("AccessMode of employee is already User", nameof(employee.AccessMode));

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    new UserDal(connection).Insert(employee.Id.Value, accessMode, username, Session.DefaultPassword);
                }
                scope.Complete();
            }

            // Returns user if successful
            return new User(employee.Id.Value, accessMode, username);
        }

        /// <summary>
        ///     Elevate or lower user access privileges to user and return if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accessMode"></param>
        public void UpdateUserAccess(User user, AccessMode accessMode)
        {
            if (user.AccessMode == accessMode)
                throw new ArgumentException(@"User already has the passed access mode");

            using (var connection = Connector.GetConnection())
            {
                if (user.AccessMode != AccessMode.None && accessMode == AccessMode.None)
                    new UserDal(connection).Delete(user.EmployeeId);
                else
                    new UserDal(connection).Update(user.EmployeeId, accessMode);
                user.AccessMode = accessMode;
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
                return new UserDal(connection).Search(employeeId).SingleOrDefault();
            }
        }

        /// <summary>
        ///     Check if username is available
        /// </summary>
        public bool IsUsernameAvailable(string username)
        {
            using (var connection = Connector.GetConnection())
            {
                return !new UserDal(connection).Search(username: username).Any();
            }
        }
    }
}