using MySql.Data.MySqlClient;

namespace Core.Data
{
    public static class ConnectionManager
    {
        private const string MacConnectionString =
            "server=localhost.mac;user id=goldline;password=1234;persistsecurityinfo=True;database=goldline";

        private const string MySqlConnectionString =
            "server=localhost;user id=goldline;password=1234;persistsecurityinfo=True;database=goldline";

        public static MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(MacConnectionString);
            connection.Open();
            return connection;
        }
    }
}