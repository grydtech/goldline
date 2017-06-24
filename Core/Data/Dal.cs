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

        internal Dal(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        ///     Returns the last inserted id value to the database through this connection
        /// </summary>
        /// <returns></returns>
        internal uint GetLastInsertId()
        {
            var command = new CommandDefinition("select last_insert_id()");
            return Connection.QuerySingle<uint>(command);
        }
    }
}