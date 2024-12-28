using DoAn.Areas.Admin.Database;
using DoAn.Models.Momo;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class CallBackDAL
    {
        DBconnect connect = new DBconnect();
        public List<Momoinfo> getall()
        {
            connect.openConnection();
            List<Momoinfo> list = new List<Momoinfo>();
            using(SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select * from momoinfo";
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Momoinfo momoinfo = new Momoinfo()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        OrderId = reader["OrderId"].ToString() ?? "",
                        OrderInfo = reader["OrderInfo"].ToString() ?? "",
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        datetime = Convert.ToDateTime(reader["datetime"])
                    };
                    list.Add(momoinfo);
                }
               
            }
            connect.closeConnection();
            return list;
        }
    }
}
