using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class CustomerDAL
    {
        DBconnect connect = new DBconnect();
        public Customer? GetCustomerById(int Id)
        {
            connect.openConnection();
            Customer customer = new Customer();
            bool hasData = false;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select * from customer where customer.id = @Id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    hasData = true;
                    customer = new Customer()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        LastName = reader["Lastname"].ToString() ?? "",
                        FirstName = reader["FirstName"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Phone = reader["Phone"].ToString() ?? "",
                        DateOfBirth = reader["dateOfBirth"] == DBNull.Value ? null
                   : DateOnly.FromDateTime(DateTime.Parse(reader["dateOfBirth"].ToString() ?? "")),
                        Img = reader["Img"].ToString() ?? "",
                        Password = reader["Password"].ToString() ?? "",
                        RandomKey = reader["RandomKey"].ToString() ?? "",
                        IsActive = Convert.ToInt32(reader["IsActive"]),
                        Role = Convert.ToInt32(reader["Role"]),
                    };
                }
            }
            connect.closeConnection();
            if (!hasData) return null;
            return customer;
        }
        public bool UpdateDetailCustomer(Customer customerUpdate, int Id)
        {
            connect.openConnection();
            int isSuccess = 0;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"update customer
                     set lastName = @LastName, firstName = @FirstName, email =
                    @Email, phone = @Phone,
                     img= @Img, address= @Address, dateOfBirth= @DateOfBirth,
                    updateAt = @updateAt
                     where id = @id
                     ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@LastName",
               customerUpdate.LastName);
                command.Parameters.AddWithValue("@FirstName",
               customerUpdate.FirstName);
                command.Parameters.AddWithValue("@Email", customerUpdate.Email);
                command.Parameters.AddWithValue("@Phone", customerUpdate.Phone);
                command.Parameters.AddWithValue("@Img", customerUpdate.Img);
                command.Parameters.AddWithValue("@Address", customerUpdate.Address);
                command.Parameters.AddWithValue("@DateOfBirth",
               customerUpdate.DateOfBirth);
                command.Parameters.AddWithValue("@updateAt",
               customerUpdate.UpdateAt);
                isSuccess = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return isSuccess > 0;
        }
        public bool SignUp(Customer customer)
        {
            connect.openConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"insert into customer 
                        (lastName, firstName, phone, email, dateOfBirth, 
                        img, address, registeredAt, updateAt, password, randomKey, isActive, role)
                    values 
                        (@lastName, @firstName, @phone, @email, @dateOfBirth, 
                        @img, @address, @registeredAt, @updateAt, @password, @randomKey, 1 , 0); ";

                command.CommandText = query;

                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@email", customer.Email);
                command.Parameters.AddWithValue("@phone", customer.Phone);
                command.Parameters.AddWithValue("@img", customer.Img);
                command.Parameters.AddWithValue("@address", customer.Address);
                command.Parameters.AddWithValue("@dateOfBirth", customer.DateOfBirth == null ? DBNull.Value : customer.DateOfBirth);
                command.Parameters.AddWithValue("@registeredAt", customer.RegisterAt);
                command.Parameters.AddWithValue("@updateAt", customer.UpdateAt);
                command.Parameters.AddWithValue("@password", customer.Password);
                command.Parameters.AddWithValue("@randomKey", customer.RandomKey);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return isSuccess > 0;
        }

        public Customer? GetCustomerByEmail(string email)
        {
            connect.openConnection();
            Customer customer = new Customer();
            bool hasData = false;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select * from customer where customer.email = @email";

                command.CommandText = query;
                command.Parameters.AddWithValue("@email", email);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hasData = true;
                    customer = new Customer()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        LastName = reader["Lastname"].ToString() ?? "",
                        FirstName = reader["FirstName"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Phone = reader["Phone"].ToString() ?? "",
                        DateOfBirth = reader["dateOfBirth"] == DBNull.Value ? null
                            : DateOnly.FromDateTime(DateTime.Parse(reader["dateOfBirth"]?.ToString())),
                        Img = reader["Img"].ToString() ?? "",
                        Password = reader["Password"].ToString() ?? "",
                        RandomKey = reader["RandomKey"].ToString() ?? "",
                        IsActive = Convert.ToInt32(reader["IsActive"]),
                        Role = Convert.ToInt32(reader["Role"]),
                    };
                }
            }
            connect.closeConnection();

            if (!hasData) return null;
            return customer;
        }


    }

}
