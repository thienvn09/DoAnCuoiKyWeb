using DoAn.Areas.Admin.Database;
using DoAn.Areas.Admin.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.Areas.DAL
{
    public class ProductAdminDAL
    {
        DBconnect connect = new DBconnect();
        public List<ProductAdmin> getAll()
        {
            connect.openConnection();

            List<ProductAdmin> list = new List<ProductAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select a.id as ProductId, a.title as ProductTitle,   
                    a.content as ProductContent, a.img as ProductImg, a.price as ProductPrice, 
                    a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,  
                    b.id as CategoryId, b.title as CategoryTitle 
                    from product a join category b on a.categoryId = b.Id 
                    ";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // ================== Cách 1 =============== 
                    ProductAdmin product = new ProductAdmin()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    // ================= Cách 2 =============== 
                    //Product product = new Product() 
                    //{ 
                    //    Id = Convert.ToInt32(reader[0]), 
                    //    Title = reader[1].ToString() ?? "", 
                    //    Content = reader[2].ToString() ?? "", 
                    //    Img = reader[3].ToString() ?? "", 
                    //    Price = Convert.ToInt32(reader[4]), 
                    //    Discount = Convert.ToDouble(reader[5].ToString()), 
                    //    CreateAt = DateTime.Parse(reader[6]?.ToString()), 
                    //    UpdateAt = DateTime.Parse(reader[7]?.ToString()), 
                    //    CategoryId = Convert.ToInt32(reader[8]), 
                    //    CategoryTitle = reader[9].ToString(), 
                    //}; 

                    list.Add(product);
                }
            }
            connect.closeConnection();
            return list;
        }
        public bool AddNew(ProductFormAdmin productNew)
        {
            connect.openConnection();

            int id = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"insert into product (title, content, img, price, rate, createAt, updateAt, categoryId) 
                    values (@title, @content, @img, @price, @rate, @createAt, @updateAt, @categoryId); ";

                command.CommandText = query;

                command.Parameters.AddWithValue("@title", productNew.Title);
                command.Parameters.AddWithValue("@content", productNew.Content);
                command.Parameters.AddWithValue("@img", productNew.Img);
                command.Parameters.AddWithValue("@price", productNew.Price);
                command.Parameters.AddWithValue("@rate", productNew.Rate);
                command.Parameters.AddWithValue("@createAt", productNew.CreateAt);
                command.Parameters.AddWithValue("@updateAt", productNew.UpdateAt);
                command.Parameters.AddWithValue("@categoryId", productNew.CategoryId);

                Console.WriteLine("command Insert Product: " + command.CommandText);

                id = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return id > 0;
        }
        public ProductAdmin GetProductById(int Id)
        {
            connect.openConnection();
            ProductAdmin product = new ProductAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select a.id as ProductId, a.title as ProductTitle,   
                    a.content as ProductContent, a.img as ProductImg, a.price as ProductPrice, 
                    a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,  
                    b.id as CategoryId, b.title as CategoryTitle 
                    from product a join category b on a.categoryId = b.Id 
                    where a.Id = @Id  ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    product = new ProductAdmin()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };
                }
            }
            connect.closeConnection();
            return product;
        }
        public bool UpdateProduct(ProductFormAdmin productNew, int Id)
        {
            connect.openConnection();
            int isSuccess = 0;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"update product
                     set title = @title, content = @content, img = @img, price =
                    @price, rate = @rate, updateAt = @updateAt, categoryId = @categoryId
                     where id = @id
                    ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@title", productNew.Title);
                command.Parameters.AddWithValue("@content", productNew.Content);
                command.Parameters.AddWithValue("@img", productNew.Img);
                command.Parameters.AddWithValue("@price", productNew.Price);
                command.Parameters.AddWithValue("@rate", productNew.Rate);
                command.Parameters.AddWithValue("@updateAt", productNew.UpdateAt);
                command.Parameters.AddWithValue("@categoryId",
               productNew.CategoryId);
                isSuccess = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return isSuccess > 0;
        }
        public bool DeleteProduct(int Id)
        {
            connect.openConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM product  
                    WHERE id = @id ";

                command.CommandText = query;

                command.Parameters.AddWithValue("@id", Id);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return isSuccess > 0;
        }
        // Lấy danh sách sản phầm theo phân trang (số trang hiện tại, số hàng trong 1 trang)
        public List<ProductAdmin> getProduct_Pagination(int pageIndex, int pageSize)
        {
            connect.openConnection();

            List<ProductAdmin> list = new List<ProductAdmin>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                //Truy vấn lồng nhau
                // ROW_NUMBER() OVER(ORDER BY a.id asc) AS RowNumber tạo thêm 1 cột để lưu index (tương tự số thứ tự)
                // sau đó sử dụng câu lệnh BETWEEN(start, end) để lấy dữ liệu có RowNumber (index ở trên): 
                //      start <= RowNumber <= end
                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY a.id asc) AS RowNumber,
                             a.id as ProductId, a.title as ProductTitle,  
                             a.content as ProductContent, a.img as ProductImg, a.price as ProductPrice,
                             a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt, 
                             b.id as CategoryId, b.title as CategoryTitle
                             FROM product a join category b on a.categoryId = b.Id	
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductAdmin product = new ProductAdmin()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    list.Add(product);
                }
            }
            connect.closeConnection();

            return list;
        }


        // Lấy danh sách sản phầm theo phân trang (số trang hiện tại, số hàng trong 1 trang)
        public List<ProductAdmin> getProduct_Pagination(int pageIndex, int pageSize, string? searchString, string sortOrder)
        {
            connect.openConnection();

            List<ProductAdmin> list = new List<ProductAdmin>();
            string condition = "";

            if (!string.IsNullOrEmpty(searchString))
            {
                condition = @" WHERE a.title LIKE '%' + @SearchString + '%' OR a.content LIKE '%' + @SearchString + '%' ";
            }

            // Truy vấn Sắp xếp
            string sortQuery = " ORDER BY a.id ";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "id_desc":
                        sortQuery = " ORDER BY a.id DESC ";
                        break;
                    case "title":
                        sortQuery = " ORDER BY a.title ";
                        break;
                    case "title_desc":
                        sortQuery = " ORDER BY a.title DESC ";
                        break;
                    case "price":
                        sortQuery = " ORDER BY a.price ";
                        break;
                    case "price_desc":
                        sortQuery = " ORDER BY a.price DESC ";
                        break;
                    case "rate":
                        sortQuery = " ORDER BY a.rate ";
                        break;
                    case "rate_desc":
                        sortQuery = " ORDER BY a.rate DESC ";
                        break;
                    default:
                        sortQuery = " ORDER BY a.id ";
                        break;
                }
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                // Truy vấn lồng nhau cho phân trang
                string query = @"
            SELECT * FROM (
                SELECT ROW_NUMBER() OVER(" + sortQuery + @") AS RowNumber,
                    a.id AS ProductId, a.title AS ProductTitle,
                    a.content AS ProductContent, a.img AS ProductImg, a.price AS ProductPrice,
                    a.rate AS ProductRate, a.createAt AS ProductCreateAt, a.updateAt AS ProductUpdateAt,
                    b.id AS CategoryId, b.title AS CategoryTitle
                FROM product a
                JOIN category b ON a.categoryId = b.Id
                " + condition + @"
            ) AS TableResult
            WHERE TableResult.RowNumber BETWEEN (@PageIndex - 1) * @PageSize + 1 AND @PageIndex * @PageSize;
        ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@SearchString", searchString ?? string.Empty);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductAdmin product = new ProductAdmin()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Content = reader["ProductContent"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"]),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString() ?? DateTime.MinValue.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString() ?? DateTime.MinValue.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    list.Add(product);
                }
            }
            connect.closeConnection();

            return list;
        }


        // Lấy số lượng kết quả sau khi truy vấn có thêm điều kiện (Search)
        public int getCountRow_Pagination(int pageIndex, int pageSize, string? searchString)
        {
            connect.openConnection();

            int count = 0;

            string condition = "";
            if (!string.IsNullOrEmpty(searchString))
            {
                condition = @" WHERE a.title LIKE '%' + @SearchString + '%' OR a.content LIKE '%' + @SearchString + '%' ";
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"
            SELECT COUNT(*) AS CountRow
            FROM product a
            JOIN category b ON a.categoryId = b.Id
            " + condition;

                command.CommandText = query;
                command.Parameters.AddWithValue("@SearchString", searchString ?? string.Empty);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = Convert.ToInt32(reader["CountRow"]);
                }
            }
            connect.closeConnection();

            return count;
        }




    }
}
