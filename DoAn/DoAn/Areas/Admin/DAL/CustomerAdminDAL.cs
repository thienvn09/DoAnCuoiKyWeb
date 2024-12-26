using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.Areas.Admin.DAL
{
    public class CustomerAdminDAL
    {
        DBconnect connect = new DBconnect();

        public List<Customer> getAll()
        {
            connect.openConnection();

            List<Customer> list = new List<Customer>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT 
                                 a.Id AS CustomerId, 
                                 a.FirstName AS CustomerFirstName,   
                                 a.LastName AS CustomerLastName, 
                                 a.Address AS CustomerAddress, 
                                 a.Phone AS CustomerPhone, 
                                 a.Email AS CustomerEmail, 
                                 a.Img AS CustomerImg, 
                                 a.DateOfBirth AS CustomerDateOfBirth, 
                                 a.Password AS CustomerPassword, 
                                 a.RandomKey AS CustomerRandomKey, 
                                 a.registeredAt AS CustomerRegisterAt, 
                                 a.updateAt AS CustomerUpdateAt, 
                                 a.IsActive AS CustomerIsActive, 
                                 a.Role AS CustomerRole
                                 FROM customer a";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Khởi tạo đối tượng CustomerAdmin từ dữ liệu đọc được
                    Customer customer = new Customer()
                    {
                        Id = Convert.ToInt32(reader["CustomerId"]),
                        FirstName = reader["CustomerFirstName"].ToString() ?? "",
                        LastName = reader["CustomerLastName"].ToString() ?? "",
                        Address = reader["CustomerAddress"].ToString() ?? "",
                        Phone = reader["CustomerPhone"].ToString() ?? "",
                        Email = reader["CustomerEmail"].ToString() ?? "",
                        Img = reader["CustomerImg"].ToString() ?? "",
                        DateOfBirth = reader["CustomerDateOfBirth"] != DBNull.Value
                            ? DateOnly.FromDateTime((DateTime)reader["CustomerDateOfBirth"])
                            : (DateOnly?)null,
                        Password = reader["CustomerPassword"].ToString() ?? "",
                        RandomKey = reader["CustomerRandomKey"].ToString() ?? "",
                        RegisterAt = reader["CustomerRegisterAt"] != DBNull.Value
                            ? Convert.ToDateTime(reader["CustomerRegisterAt"])
                            : DateTime.MinValue,
                        UpdateAt = reader["CustomerUpdateAt"] != DBNull.Value
                            ? Convert.ToDateTime(reader["CustomerUpdateAt"])
                            : DateTime.MinValue,
                        IsActive = reader["CustomerIsActive"] != DBNull.Value
                            ? Convert.ToBoolean(reader["CustomerIsActive"]) ? 1 : 0 // Chuyển đổi từ bool sang int
                            : 0, // Nếu giá trị là DBNull, gán mặc định là 0
                        Role = reader["CustomerRole"] != DBNull.Value
                            ? Convert.ToInt32(reader["CustomerRole"])
                            : 0,
                    };

                    list.Add(customer);
                }
            }

            connect.closeConnection();
            return list;
        }
        public bool AddNew(Customer customerNew)
        {
            connect.openConnection();
            int result = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"INSERT INTO customer (FirstName, LastName, Address, Phone, Email, Img, DateOfBirth, Password, RandomKey, RegisterAt, UpdateAt, IsActive, Role) 
                         VALUES (@FirstName, @LastName, @Address, @Phone, @Email, @Img, @DateOfBirth, @Password, @RandomKey, @RegisterAt, @UpdateAt, @IsActive, @Role)";

                command.CommandText = query;

                command.Parameters.AddWithValue("@FirstName", customerNew.FirstName);
                command.Parameters.AddWithValue("@LastName", customerNew.LastName);
                command.Parameters.AddWithValue("@Address", customerNew.Address);
                command.Parameters.AddWithValue("@Phone", customerNew.Phone);
                command.Parameters.AddWithValue("@Email", customerNew.Email);
                command.Parameters.AddWithValue("@Img", customerNew.Img);
                command.Parameters.AddWithValue("@DateOfBirth", customerNew.DateOfBirth);
                command.Parameters.AddWithValue("@Password", customerNew.Password);
                command.Parameters.AddWithValue("@RandomKey", customerNew.RandomKey);
                command.Parameters.AddWithValue("@RegisterAt", customerNew.RegisterAt);
                command.Parameters.AddWithValue("@UpdateAt", customerNew.UpdateAt);
                command.Parameters.AddWithValue("@IsActive", customerNew.IsActive);
                command.Parameters.AddWithValue("@Role", customerNew.Role);

                result = command.ExecuteNonQuery();
            }

            connect.closeConnection();
            return result > 0;
        }
        public Customer GetCustomerById(int id)
        {
            connect.openConnection();
            Customer customer = null;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT 
            a.Id AS CustomerId, a.FirstName AS CustomerFirstName, a.LastName AS CustomerLastName,
            a.Address AS CustomerAddress, a.Phone AS CustomerPhone, a.Email AS CustomerEmail,
            a.Img AS CustomerImg, a.DateOfBirth AS CustomerDateOfBirth, a.Password AS CustomerPassword,
            a.RandomKey AS CustomerRandomKey, a.RegisterAt AS CustomerRegisterAt, a.UpdateAt AS CustomerUpdateAt,
            a.IsActive AS CustomerIsActive, a.Role AS CustomerRole
            FROM customer a
            WHERE a.Id = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer()
                    {
                        Id = Convert.ToInt32(reader["CustomerId"]),
                        FirstName = reader["CustomerFirstName"].ToString() ?? "",
                        LastName = reader["CustomerLastName"].ToString() ?? "",
                        Address = reader["CustomerAddress"].ToString() ?? "",
                        Phone = reader["CustomerPhone"].ToString() ?? "",
                        Email = reader["CustomerEmail"].ToString() ?? "",
                        Img = reader["CustomerImg"].ToString() ?? "",
                        DateOfBirth = reader["CustomerDateOfBirth"] != DBNull.Value
                            ? DateOnly.FromDateTime((DateTime)reader["CustomerDateOfBirth"])
                            : (DateOnly?)null,
                        Password = reader["CustomerPassword"].ToString() ?? "",
                        RandomKey = reader["CustomerRandomKey"].ToString() ?? "",
                        RegisterAt = reader["CustomerRegisterAt"] != DBNull.Value
                            ? Convert.ToDateTime(reader["CustomerRegisterAt"])
                            : DateTime.MinValue,
                        UpdateAt = reader["CustomerUpdateAt"] != DBNull.Value
                            ? Convert.ToDateTime(reader["CustomerUpdateAt"])
                            : DateTime.MinValue,
                        IsActive = reader["CustomerIsActive"] != DBNull.Value ? Convert.ToInt32(reader["CustomerIsActive"]) : 0,
                        Role = reader["CustomerRole"] != DBNull.Value ? Convert.ToInt32(reader["CustomerRole"]) : 0,
                    };
                }
            }

            connect.closeConnection();
            return customer;
        }
        public bool UpdateCustomer(Customer customerNew, int id)
        {
            connect.openConnection();
            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE customer 
                         SET FirstName = @FirstName, LastName = @LastName, Address = @Address, 
                             Phone = @Phone, Email = @Email, Img = @Img, DateOfBirth = @DateOfBirth, 
                             Password = @Password, RandomKey = @RandomKey, RegisterAt = @RegisterAt, 
                             UpdateAt = @UpdateAt, IsActive = @IsActive, Role = @Role 
                         WHERE Id = @Id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FirstName", customerNew.FirstName);
                command.Parameters.AddWithValue("@LastName", customerNew.LastName);
                command.Parameters.AddWithValue("@Address", customerNew.Address);
                command.Parameters.AddWithValue("@Phone", customerNew.Phone);
                command.Parameters.AddWithValue("@Email", customerNew.Email);
                command.Parameters.AddWithValue("@Img", customerNew.Img);
                command.Parameters.AddWithValue("@DateOfBirth", customerNew.DateOfBirth);
                command.Parameters.AddWithValue("@Password", customerNew.Password);
                command.Parameters.AddWithValue("@RandomKey", customerNew.RandomKey);
                command.Parameters.AddWithValue("@RegisterAt", customerNew.RegisterAt);
                command.Parameters.AddWithValue("@UpdateAt", customerNew.UpdateAt);
                command.Parameters.AddWithValue("@IsActive", customerNew.IsActive);
                command.Parameters.AddWithValue("@Role", customerNew.Role);

                isSuccess = command.ExecuteNonQuery();
            }

            connect.closeConnection();
            return isSuccess > 0;
        }
        public bool DeleteCustomer(int id)
        {
            connect.openConnection();
            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM customer WHERE Id = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);

                isSuccess = command.ExecuteNonQuery();
            }

            connect.closeConnection();
            return isSuccess > 0;
        }

    }
}
