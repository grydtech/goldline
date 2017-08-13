using System;
using System.Collections.Generic;
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
        ///     Inserts a record into [users] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="accessMode"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Insert(uint employeeId, AccessMode accessMode, string username, string password)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into users (id_employee, type_user, username, password) " +
                "values(@employeeId, @accessMode, @username, @password)",
                new {employeeId, accessMode, username, password});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [users] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IEnumerable<User> Search(uint? employeeId = null, string username = null, string password = null,
            int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_employee 'EmployeeId', type_user-0 'AccessMode', username 'Username', password 'Password' " +
                "from users " +
                (employeeId == null && username == null && password == null ? "" : "where ") +
                (employeeId == null ? "" : "id_employee = @employeeId ") +
                (username == null ? "" : (employeeId == null ? "" : "and ") + "username = @username ") +
                (password == null
                    ? ""
                    : (employeeId == null && username == null ? "" : "and ") + "password = @password ") +
                "limit @offset, @limit",
                new {employeeId, username, password, offset, limit});

            // Execute sql command
            return Connection.Query<User>(command);
        }

        /// <summary>
        ///     Updates a record in [users] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="accessMode"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Update(uint employeeId, AccessMode? accessMode = null, string username = null,
            string password = null)
        {
            if (accessMode == null && username == null && password == null)
                throw new ArgumentNullException(nameof(Update), @"No update parameters were passed.");

            // Define sql command
            var command = new CommandDefinition(
                "update users set " +
                ((accessMode == null ? "" : "type_user = @accessMode, ") +
                 (username == null ? "" : "username = @username, ") +
                 (password == null ? "" : "password = @password, ")).TrimEnd(' ', ',') +
                " where id_employee = @employeeId",
                new {employeeId, accessMode, username, password});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [users] table
        /// </summary>
        /// <param name="employeeId"></param>
        public void Delete(uint employeeId)
        {
            // Define sql command
            var command = new CommandDefinition("delete from users where id_employee = @employeeId", new {employeeId});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}