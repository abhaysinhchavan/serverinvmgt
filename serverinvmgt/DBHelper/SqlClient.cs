using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace serverinvmgt.DBHelper
{
    public class SqlClient
    {

        /// <summary>
        /// The connection string for sql server.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The timeout for a sql operation
        /// </summary>
        private int timeout = 180;

        /// <summary>
        /// Initializes a new instance of the sqlClient class.
        /// </summary>
        public SqlClient()
        {
            connectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        /// <summary>
        /// Initializes a new instance of the sqlClient class.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <param name="queryTimeout">Query timeout in seconds.</param>
        public SqlClient(string host, int queryTimeout = 180)
        {
            connectionString = host;
            timeout = queryTimeout;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns>Datable containing query results</returns>
        /// <param name="sqlQuery">Sql query.</param>
        /// <param name="parameters">Parameters.</param>
        public DataTable ExecuteQuery(string sqlQuery, Dictionary<string, object> parameters)
        {
            DataTable dataTable = new DataTable();
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = timeout
                    };

                    foreach (KeyValuePair<string, object> paramater in parameters)
                    {
                        command.Parameters.AddWithValue(paramater.Key, paramater.Value);
                    }

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                }
            }
            catch (Exception ex)
            {
                return dataTable;
            }

            return dataTable;
        }

        public async Task<DataTable> ExecuteQueryAsync(string sqlQuery, Dictionary<string, string> parameters)
        {
            DataTable dataTable = new DataTable();
            Dictionary<string, string> queryParams = parameters ?? new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return dataTable;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = timeout
                    };

                    foreach (KeyValuePair<string, string> paramater in queryParams)
                    {
                        command.Parameters.AddWithValue(paramater.Key, paramater.Value);
                    }

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                return dataTable;
            }
        }

        public async Task<bool> ExecuteStoredProcedureAsync(string storedProcedure, Dictionary<string, object> parameters)
        {
            DataTable dataTable = new DataTable();
            Dictionary<string, object> queryParams = parameters ?? new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return false;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(storedProcedure, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = timeout
                    };

                    foreach (KeyValuePair<string, object> paramater in queryParams)
                    {
                        command.Parameters.AddWithValue(paramater.Key, paramater.Value);
                    }

                    connection.Open();
                    await command.ExecuteReaderAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ExecuteNonQueryAsync(string strQuery)
        {
            bool blnReturn = false;

            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return blnReturn;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(strQuery, connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = timeout
                    };

                    connection.Open();
                    command.ExecuteReaderAsync();
                    blnReturn = true;
                }
            }
            catch (Exception ex)
            {
            }
            return blnReturn;
        }

        public string ExecuteScalar(string strQuery)
        {
            string Return = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return Return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(strQuery, connection)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = timeout
                    };

                    connection.Open();
                    Return = Convert.ToString(command.ExecuteScalar()); 
                }
            }
            catch (Exception ex)
            {
            }
            return Return;
        }
    }
}