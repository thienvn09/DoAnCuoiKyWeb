using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class PaymentDAL
    {
        public DBconnect _dbconnect = new DBconnect();
        // Hàm lưu thông tin thanh toán vào bảng PaymentInfo
        public void SavePayment(PaymentInfo payment)
        {
            try
            {
                _dbconnect.openConnection();
                string query = @"INSERT INTO PaymentInfo 
                                 (OrderId, RequestId, Amount, OrderInfo, TransId, ResponseTime, ResultCode, Message) 
                                 VALUES 
                                 (@OrderId, @RequestId, @Amount, @OrderInfo, @TransId, @ResponseTime, @ResultCode, @Message)";
                using (SqlCommand command = new SqlCommand(query, _dbconnect.getConnecttion()))
                {
                    command.Parameters.AddWithValue("@OrderId", payment.OrderId);
                    command.Parameters.AddWithValue("@RequestId", payment.RequestId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@OrderInfo", payment.OrderInfo);
                    command.Parameters.AddWithValue("@TransId", payment.TransId);
                    command.Parameters.AddWithValue("@ResponseTime", payment.ResponseTime);
                    command.Parameters.AddWithValue("@ResultCode", payment.ResultCode);
                    command.Parameters.AddWithValue("@Message", payment.Message);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                throw new Exception("Lỗi khi lưu thanh toán: " + ex.Message);
            }
            finally
            {
                _dbconnect.closeConnection();
            }
        }

        // Hàm lấy danh sách tất cả giao dịch từ PaymentInfo
        public List<PaymentInfo> GetAllPayments()
        {
            List<PaymentInfo> payments = new List<PaymentInfo>();
            try
            {
                _dbconnect.openConnection();
                string query = "SELECT * FROM PaymentInfo";
                using (SqlCommand command = new SqlCommand(query, _dbconnect.getConnecttion()))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        payments.Add(new PaymentInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            OrderId = reader["OrderId"].ToString(),
                            RequestId = reader["RequestId"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            OrderInfo = reader["OrderInfo"].ToString(),
                            TransId = reader["TransId"].ToString(),
                            ResponseTime = Convert.ToDateTime(reader["ResponseTime"]),
                            ResultCode = Convert.ToInt32(reader["ResultCode"]),
                            Message = reader["Message"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần thiết
                throw new Exception("Lỗi khi lấy danh sách thanh toán: " + ex.Message);
            }
            finally
            {
                _dbconnect.closeConnection();
            }
            return payments;
        }
    }
}
