using System.Data;
using Dapper;

namespace Core.Datalayer
{
    /// <summary>
    ///     Base class for all Dal classes.
    /// </summary>
    internal abstract class Dal
    {
        protected internal readonly IDbConnection Connection;

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
            return Connection.ExecuteScalar<uint>(command);
        }
    }
}