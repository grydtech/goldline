using MySql.Data.MySqlClient;

namespace Core.Data
{
    public static class Connector
    {
        private const string ConStringMac =
            "server=localhost.mac;user id=goldline;password=1234;persistsecurityinfo=True;database=goldline";

        private const string ConStringLocal =
            "server=localhost;user id=goldline;password=1234;persistsecurityinfo=True;database=goldline";

        public static MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(ConStringMac);
                connection.Open();
                return connection;
            }
            catch (MySqlException)
            {
                var connection = new MySqlConnection(ConStringLocal);
                connection.Open();
                return connection;
            }
        }
    }
}