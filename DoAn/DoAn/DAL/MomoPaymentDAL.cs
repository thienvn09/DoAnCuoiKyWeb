using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class MomoPaymentDAL
    {
        private DBconnect _dbConnect = new DBconnect();  // Sử dụng đối tượng DBconnect

        // Thêm bản ghi thanh toán vào bảng MomoPaymentAdmin
        public async Task<bool> AddMomoPaymentAsync(MomoPayment momoPayment)
        {
            string query = @"
                INSERT INTO MomoPayment
                (CustomerId, FirstName, LastName, Phone, Email, CreateAt, Total, MomoTransactionId, PayUrl, PaymentStatus, PaymentDate, OrderInfo)
                VALUES
                (@CustomerId, @FirstName, @LastName, @Phone, @Email, @CreateAt, @Total, @MomoTransactionId, @PayUrl, @PaymentStatus, @PaymentDate, @OrderInfo)";

            using (var connection = _dbConnect.getConnecttion()) // Sử dụng connection từ DBconnect
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", momoPayment.CustomerId);
                    command.Parameters.AddWithValue("@FirstName", momoPayment.FirstName);
                    command.Parameters.AddWithValue("@LastName", momoPayment.LastName);
                    command.Parameters.AddWithValue("@Phone", momoPayment.Phone);
                    command.Parameters.AddWithValue("@Email", momoPayment.Email);
                    command.Parameters.AddWithValue("@CreateAt", momoPayment.CreateAt);
                    command.Parameters.AddWithValue("@Total", momoPayment.Total);
                    command.Parameters.AddWithValue("@MomoTransactionId", momoPayment.MomoTransactionId);
                    command.Parameters.AddWithValue("@PayUrl", momoPayment.PayUrl);
                    command.Parameters.AddWithValue("@PaymentStatus", momoPayment.PaymentStatus);
                    command.Parameters.AddWithValue("@PaymentDate", momoPayment.PaymentDate);
                    command.Parameters.AddWithValue("@OrderInfo", momoPayment.OrderInfo);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;  // Trả về true nếu có ít nhất một bản ghi được thêm vào
                }
            }
        }
        public async Task<List<MomoPayment>> GetMomoPaymentsAsync()
        {
            var payments = new List<MomoPayment>();
            string query = "SELECT * FROM MomoPayment";

            using (var connection = _dbConnect.getConnecttion())  // Sử dụng connection từ DBconnect
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        payments.Add(new MomoPayment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            CreateAt = reader.GetDateTime(reader.GetOrdinal("CreateAt")),
                            Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? (float?)null : reader.GetFloat(reader.GetOrdinal("Total")),
                            MomoTransactionId = reader.GetString(reader.GetOrdinal("MomoTransactionId")),
                            PayUrl = reader.GetString(reader.GetOrdinal("PayUrl")),
                            PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                            PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                            OrderInfo = reader.GetString(reader.GetOrdinal("OrderInfo"))
                        });
                    }
                }
            }

            return payments;
        }
    }
}
