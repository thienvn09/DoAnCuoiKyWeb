using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class CategoryDAL
    {
        DBconnect connect = new DBconnect();
        public List<CategoryMenu> getAllWithCount()
        {
            connect.openConnection();
            List<CategoryMenu> list = new List<CategoryMenu>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select c.id, c.title, c.content, count(p.id) as
                            soluong
                             from category c left join product p
                            on c.id = p.categoryId
                            group by c.id,c.title, c.content
                            order by soluong desc
                            ";
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CategoryMenu category = new CategoryMenu()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString() ?? "",
                        Content = reader["Content"].ToString() ?? "",
                        Count = Convert.ToInt32(reader["soluong"].ToString())
                    };
                    list.Add(category);
                }
            }
            connect.closeConnection();
            return list;
        }

    }
}
