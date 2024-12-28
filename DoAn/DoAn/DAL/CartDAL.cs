using DoAn.Areas.Admin.Database;
using DoAn.Models;
using Microsoft.Data.SqlClient;

namespace DoAn.DAL
{
    public class CartDAL
    {
        DBconnect connect = new DBconnect();

        // Kiểm tra và tạo đơn hàng
        public bool CheckOut(Customer customer, List<CartItem> cart)
        {
            connect.openConnection();
            bool insertPaymentSuccess = false;
            bool CheckOutSuccess = true;
            int? paymentId = null;
            int totalCartAmount = cart.Sum(p => p.Total); // Tổng giỏ hàng

            // Insert vào bảng Payment và lấy Id tự động sau khi insert
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"
                    INSERT INTO payment (customerId, firstName, lastName, phone, email, createAt, total)
                    VALUES (@customerId, @firstName, @lastName, @phone, @email, GETDATE(), @total);
                    SELECT SCOPE_IDENTITY() AS PaymentId;
                ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@customerId", customer.Id);
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@phone", customer.Phone);
                command.Parameters.AddWithValue("@email", customer.Email);
                command.Parameters.AddWithValue("@total", totalCartAmount);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    insertPaymentSuccess = true;
                    paymentId = reader["PaymentId"] == DBNull.Value ? null : Convert.ToInt32(reader["PaymentId"]);
                }
            }
            connect.closeConnection();

            // Nếu không thành công trong việc insert Payment thì trả về false
            if (!insertPaymentSuccess || paymentId == null)
            {
                return false;
            }

            // Tiến hành thêm chi tiết đơn hàng vào bảng PaymentDetail
            foreach (var item in cart)
            {
                bool detailInsertSuccess = InsertToPaymentDetail(paymentId.Value, item);
                if (!detailInsertSuccess)
                {
                    CheckOutSuccess = false;
                }
            }

            return CheckOutSuccess;
        }

        // Insert vào bảng PaymentDetail
        public bool InsertToPaymentDetail(int paymentId, CartItem itemCart)
        {
            connect.openConnection();
            int rowsAffected = 0;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"
                    INSERT INTO paymentDetail (paymentId, productId, price, quantity, total, createAt)
                    VALUES (@paymentId, @productId, @price, @quantity, @total, GETDATE());
                ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@paymentId", paymentId);
                command.Parameters.AddWithValue("@productId", itemCart.IdProduct);
                command.Parameters.AddWithValue("@price", itemCart.Price);
                command.Parameters.AddWithValue("@quantity", itemCart.Quantity);
                command.Parameters.AddWithValue("@total", itemCart.Total);

                rowsAffected = command.ExecuteNonQuery();
            }
            connect.closeConnection();
            return rowsAffected > 0; // Trả về true nếu insert thành công
        }

        public int SavePayment(Payment payment)
        {
            connect.openConnection();
            int paymentId = 0;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"
            INSERT INTO payment (customerId, firstName, lastName, phone, email, createAt, total, paymentMethodId)
            OUTPUT INSERTED.paymentId
            VALUES (@customerId, @firstName, @lastName, @phone, @email, GETDATE(), @total, @paymentMethodId);
        ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@customerId", payment.CustomerId);
                command.Parameters.AddWithValue("@firstName", payment.FirstName);
                command.Parameters.AddWithValue("@lastName", payment.LastName);
                command.Parameters.AddWithValue("@phone", payment.Phone);
                command.Parameters.AddWithValue("@email", payment.Email);
                command.Parameters.AddWithValue("@total", payment.Total);
                command.Parameters.AddWithValue("@paymentMethodId", payment.PaymentMethodId);

                paymentId = Convert.ToInt32(command.ExecuteScalar());
            }
            connect.closeConnection();
            return paymentId; // Trả về Id của payment vừa tạo
        }
        public bool UpdatePaymentStatus(string transactionId, string status)
        {
            connect.openConnection();
            bool updateSuccess = false;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.getConnecttion();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"
            UPDATE payment
            SET paymentStatus = @status
            WHERE transactionId = @transactionId;
        ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@transactionId", transactionId);
                command.Parameters.AddWithValue("@status", status);

                int rowsAffected = command.ExecuteNonQuery();
                updateSuccess = rowsAffected > 0;
            }
            connect.closeConnection();

            return updateSuccess;
        }

    }
}
