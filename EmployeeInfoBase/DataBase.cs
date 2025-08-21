using System.Data.SqlClient;

namespace EmployeeInfoBase
{
    public class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data source = (localdb)\MSSQLLocalDB; Initial catalog = EmployeeInfoBase; Integrated Security = True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
