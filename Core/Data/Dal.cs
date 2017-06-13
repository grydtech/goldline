using System.Data;
using Dapper;

namespace Core.Data
{
    /// <summary>
    ///     Base class for all Dal classes.
    /// </summary>
    internal abstract class Dal
    {
        protected readonly IDbConnection Connection;
        protected readonly IDbTransaction Transaction;

        internal Dal(IDbConnection connection, IDbTransaction transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }

        /// <summary>
        ///     Returns the last inserted id value to the database through this connection
        /// </summary>
        /// <returns></returns>
        internal uint GetLastInsertId()
        {
            var command = new CommandDefinition("select last_insert_id()", null, Transaction);
            return Connection.QuerySingle<uint>(command);
        }
    }
}