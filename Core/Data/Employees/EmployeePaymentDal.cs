using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Employees;
using Dapper;

namespace Core.Data.Employees
{
    internal class EmployeePaymentDal : Dal
    {
        internal EmployeePaymentDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [employees_payments] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        internal void Insert(uint employeeId, decimal amount, string note)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into employees_payments (id_employee, amount, note) values (@employeeId, @amount, @note)",
                new {employeeId, amount, note});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches records in [employees_payments] table
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        internal IEnumerable<EmployeePayment> Search(uint? employeeId = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_employee 'EmployeeId', date_paid 'Date', amount_paid 'Amount', note 'Note' " +
                "from employees_payments " +
                (employeeId == null ? "" : "where id_employee = @employeeId ") +
                "order by id_payment desc limit @offset, @limit",
                new {employeeId, offset, limit});

            // Execute sql command
            return Connection.Query<EmployeePayment>(command);
        }

        /// <summary>
        ///     Deletes a record from [employees_payments] table
        /// </summary>
        /// <param name="employeePaymentId"></param>
        internal void Delete(uint id)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from employees_payments where id_payment = @id",
                new {id});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}