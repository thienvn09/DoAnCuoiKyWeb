using Microsoft.Data.SqlClient;

namespace DoAn.Areas.Admin.Database
{
        public class DBconnect
        {
            //to create connection 
            SqlConnection connect = new SqlConnection("Data Source=LAPTOP-HCQ7FVIS\\SQLEXPRESS;Initial Catalog=CuoiKy_LanCuoi;Integrated Security=True;Trust Server Certificate=True");

            //to get connection 
            public SqlConnection getConnecttion()
            {
                return connect;

            }

            //create a function to Open connection 
            public void openConnection()
            {
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    connect.Open();
                }
            }

            //create a function to Close connection 
            public void closeConnection()
            {
                if (connect.State == System.Data.ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
}
