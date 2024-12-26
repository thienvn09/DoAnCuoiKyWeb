using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoAn.DAL
{
    public class ProductDAL
    {
        DBconnect connect = new DBconnect();
        public List<Product> GetListProduct(int? idCategory)
        {
            connect.openConnection();
            List<Product> list = new List<Product>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
      string query = @"select a.id as ProductId, a.title as ProductTitle,
 a.content as ProductContent, a.img as ProductImg, a.price as
ProductPrice,
 a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt
as ProductUpdateAt,
 b.id as CategoryId, b.title as CategoryTitle
from product a join category b on a.categoryId = b.Id
";

                //nếu IdCategory có giá trị thì thêm WHERE cho câu Query
                if (idCategory.HasValue)
                {
                    query = query + "where b.Id = " + idCategory;
                }
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // ================== Cách 1 ===============
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt =
                    DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt =
                    DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };
                    list.Add(product);
                }
            }
            connect.closeConnection();
            return list;
        }
        public Product GetProductById(int Id)
        {
            connect.openConnection();
            Product product = new Product();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select a.id as ProductId, a.title as ProductTitle,
 a.content as ProductContent, a.img as ProductImg, a.price as
ProductPrice,
 a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt
as ProductUpdateAt,
 b.id as CategoryId, b.title as CategoryTitle
from product a join category b on a.categoryId = b.Id
 where a.Id = @Id ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt =
                   DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt =
                   DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };
                }
            }
            connect.closeConnection();
            return product;
        }
        public List<Product> GetFeaturedProducts(int limitProduct)
        {
            connect.openConnection();
            List<Product> list = new List<Product>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"
            SELECT TOP (@LimitProduct)
                a.id AS ProductId,
                a.title AS ProductTitle,
                a.content AS ProductContent,
                a.img AS ProductImg,
                a.price AS ProductPrice,
                a.rate AS ProductRate,
                a.createAt AS ProductCreateAt,
                a.updateAt AS ProductUpdateAt,
                b.id AS CategoryId,
                b.title AS CategoryTitle
            FROM product a
            JOIN category b ON a.categoryId = b.Id
            ORDER BY a.rate DESC;
        ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@LimitProduct", limitProduct);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString(),
                        Content = reader["ProductContent"].ToString(),
                        Img = reader["ProductImg"].ToString(),
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"]),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"].ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"].ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString()
                    };
                    list.Add(product);
                }
            }
            connect.closeConnection();
            return list;
        }

        public List<Product> GetProducts_Pagination(int? idCategory, int pageIndex, int pageSize, string sortOrder)
        {
            connect.openConnection();

            List<Product> list = new List<Product>();

            // Điều kiện Category
            string categoryCondition = idCategory.HasValue ? $" WHERE a.categoryId = @CategoryId" : "";

            // Truy vấn sắp xếp
            string sortQuery = "ORDER BY a.id";  // Default sort by id
            switch (sortOrder)
            {
                case "price":
                    sortQuery = "ORDER BY a.price";
                    break;
                case "price_desc":
                    sortQuery = "ORDER BY a.price DESC";
                    break;
                case "rate_desc":
                    sortQuery = "ORDER BY a.rate DESC";
                    break;
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = $@"
            SELECT * FROM (
                SELECT ROW_NUMBER() OVER({sortQuery}) AS RowNumber,
                    a.id AS ProductId, 
                    a.title AS ProductTitle,  
                    a.content AS ProductContent, 
                    a.img AS ProductImg, 
                    a.price AS ProductPrice,
                    a.rate AS ProductRate, 
                    a.createAt AS ProductCreateAt, 
                    a.updateAt AS ProductUpdateAt, 
                    b.id AS CategoryId, 
                    b.title AS CategoryTitle
                FROM product a
                JOIN category b ON a.categoryId = b.Id
                {categoryCondition}
            ) AS TableResult
            WHERE TableResult.RowNumber BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize;
        ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                if (idCategory.HasValue)
                {
                    command.Parameters.AddWithValue("@CategoryId", idCategory.Value);
                }

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString(),
                        Content = reader["ProductContent"].ToString(),
                        Img = reader["ProductImg"].ToString(),
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"]),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"].ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"].ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString()
                    };
                    list.Add(product);
                }
            }

            connect.closeConnection();
            return list;
        }


    }

}
