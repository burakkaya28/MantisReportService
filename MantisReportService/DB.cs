using System.Configuration;
using MySql.Data.MySqlClient;

namespace MantisReportService
{
    public class Db
    {
        private MySqlConnection _con;
        private MySqlCommand _cmd;

        private static readonly string DbServer = ConfigurationManager.AppSettings["SERVER"];
        private static readonly string DbPort = ConfigurationManager.AppSettings["PORT"];
        private static readonly string DbDatabase = ConfigurationManager.AppSettings["DATABASE"];
        private static readonly string DbUser = ConfigurationManager.AppSettings["DB_USER"];
        private static readonly string DbPassword = ConfigurationManager.AppSettings["DB_PASSWORD"];

        public string GetValueFromDb(string sqlQuery)
        {
            _con = new MySqlConnection
            {
                ConnectionString = "Server=" + DbServer + ";Port=" + DbPort + ";Database=" + DbDatabase +
                                   ";Uid=" + DbUser + ";Pwd=" + DbPassword +
                                   ";Encrypt=false;AllowUserVariables=True;UseCompression=True;"
            };
            _con.Open();
            _cmd = new MySqlCommand
            {
                CommandText = sqlQuery,
                Connection = _con
            };
            var str = _cmd.ExecuteScalar().ToString();
            _con.Close();

            return str;
        }
    }
}
