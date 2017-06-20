using System.Collections.Generic;
using System.Data;
using Core.Model.Payments;
using Dapper;

namespace Core.Data
{
    internal class EmployeesPaymentsDal : Dal
    {
        internal EmployeesPaymentsDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Inserts a new employee payment into database
        /// </summary>
        /// <param name="employeePayment"></param>
        internal void InsertEmployeePayment(EmployeePayment employeePayment)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into employees_payments (id_employee, amount_paid, note) values (@id_employee, @amount, @note)",
                new
                {
                    id_employee = employeePayment.EmployeeId,
                    amount = employeePayment.Amount,
                    note = employeePayment.Note,
                }, Transaction);

            // Execute sql command
            Connection.Execute(command);

            // Assign attributes
            employeePayment.Id = GetLastInsertId();
        }

        /// <summary>
        ///     Removes given employee payment from database
        /// </summary>
        /// <param name="employeePaymentId"></param>
        internal void RemoveEmployeePayment(uint employeePaymentId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from employees_payments where id_payment = @id_payment",
                new
                {
                    id_payment = employeePaymentId
                }, Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns payments of a given Employee from database
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="recordLimit">number of employee payments returned</param>
        internal IEnumerable<EmployeePayment> GetEmployeePayments(uint employeeId, uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_employee 'EmployeeId', amount_paid 'Amount', note 'Note', date_paid 'Date' " +
                "from employees_payments where id_employee = @id_employee " +
                "order by id_payment desc limit @limit",
                new
                {
                    id_employee = employeeId,
                    limit = recordLimit
                }, Transaction);

            // Execute sql command
            return Connection.Query<EmployeePayment>(command);
        }

        /// <summary>
        ///     Returns a list of most recent payments from database
        /// </summary>
        /// <param name="recordLimit">number of employee payments returned</param>
        internal IEnumerable<EmployeePayment> GetRecentEmployeePayments(uint recordLimit)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select id_payment 'Id', id_employee 'EmployeeId', amount_paid 'Amount', note 'Note', date_paid 'Date' " +
                "from employees_payments " +
                "order by id_payment desc limit @limit",
                new
                {
                    limit = recordLimit
                }, Transaction);

            // Execute sql command
            return Connection.Query<EmployeePayment>(command);
        }
    }
}