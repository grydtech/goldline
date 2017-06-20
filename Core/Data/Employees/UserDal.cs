using System.Data;
using Core.Domain.Enums;
using Core.Domain.Model.Employees;
using Dapper;

namespace Core.Data.Employees
{
    internal class UserDal : Dal
    {
        public UserDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Returns user object if user exists for given username and either given password
        ///     or default password, else returns null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUser(string username, string password)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'EmployeeId', type_user-1 'AccessMode', username 'Username', password 'Password' " +
                "from users where username = @u and password = @p",
                new
                {
                    u = username,
                    p = password
                });

            // Execute sql command
            return Connection.QuerySingleOrDefault<User>(command);
        }

        /// <summary>
        ///     Returns user object if exists for given employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public User GetUser(uint employeeId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'EmployeeId', type_user-1 'AccessMode', username 'Username', password 'Password' " +
                "from users where id_employee = @id_employee",
                new
                {
                    id_employee = employeeId
                });

            // Execute sql command
            return Connection.QuerySingleOrDefault<User>(command);
        }

        /// <summary>
        ///     Checks if the given username is available or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUsernameAvailable(string username)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select not exists (select username from users where username = @u)",
                new {u = username});

            // Execute sql command
            return Connection.ExecuteScalar<bool>(command);
        }

        /// <summary>
        ///     Updates an existing user type in database, and assigns new userType if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newUserType">new usertype for user</param>
        public void UpdateUserType(User user, AccessMode newUserType)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update users set type_user = @type_user where id_employee = @id_employee",
                new
                {
                    id_employee = user.EmployeeId,
                    type_user = newUserType.ToString()
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            user.AccessMode = newUserType;
        }

        /// <summary>
        ///     Updates an existing user password in database, and assign new password if successful
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        public void UpdateUserPassword(User user, string newPassword)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update users set password = @password where id_employee = @id_employee",
                new
                {
                    id_employee = user.EmployeeId,
                    password = newPassword
                });

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            user.Password = newPassword;
        }

        /// <summary>
        ///     Inserts a new user into database
        /// </summary>
        /// <param name="user"></param>
        public void InsertUser(User user)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into users (id_employee, type_user, username, password) " +
                "values(@id_employee, @type_user, @username, @password)",
                new
                {
                    id_employee = user.EmployeeId,
                    type_user = user.AccessMode.ToString(),
                    username = user.Username,
                    password = user.Password
                });

            // Execute sql command
            Connection.Execute(command);
        }
    }
}