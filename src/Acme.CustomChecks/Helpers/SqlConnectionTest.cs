using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.CustomChecks.Helpers
{
    public class SqlConnectionTest
    {
        private string _connectionString;
        private string _tableToQuery;

        public SqlConnectionTest(string connectionString, string tableToQuery)
        {
            _connectionString = connectionString;
            _tableToQuery = tableToQuery;
        }

        public void Test()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var query = new SqlCommand("SELECT COUNT(1) FROM " + _tableToQuery, sqlConnection))
            {
                sqlConnection.Open();
                var result = query.ExecuteScalar();
                sqlConnection.Close();
            }
        }
    }
}
