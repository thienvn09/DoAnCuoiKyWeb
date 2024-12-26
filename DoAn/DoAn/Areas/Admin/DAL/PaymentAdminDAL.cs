using DoAn.Areas.Admin.Database;
using DoAn.Areas.Admin.Models;
using DoAn.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DoAn.Areas.Admin.DAL
{
    public class PaymentAdminDAL
    {
        private readonly DBconnect _connect = new DBconnect();

        // Lấy danh sách tất cả các Payment
        public List<PaymentAdmin> GetAllPayments()
        {
            var payments = new List<PaymentAdmin>();

            using (var connection = _connect.getConnecttion())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            p.id AS PaymentId,
                            p.customerId AS CustomerId,
                            p.firstName AS CustomerFirstName,
                            p.lastName AS CustomerLastName,
                            p.phone AS CustomerPhone,
                            p.email AS CustomerEmail,
                            p.createAt AS PaymentCreateAt,
                            p.total AS PaymentTotal,
                            pd.productId AS ProductId,
                            pd.price AS ProductPrice,
                            pd.quantity AS ProductQuantity,
                            pd.total AS PaymentDetailTotal,
                            pd.createAt AS PaymentDetailCreateAt
                        FROM 
                            payment p
                        LEFT JOIN 
                            paymentDetail pd ON p.id = pd.paymentId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            // Kiểm tra nếu Payment đã tồn tại trong danh sách
                            var paymentId = Convert.ToInt32(reader["PaymentId"]);
                            var payment = payments.Find(p => p.Id == paymentId);

                            if (payment == null)
                            {
                                payment = new PaymentAdmin
                                {
                                    Id = paymentId,
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : (int?)null,
                                    FirstName = reader["CustomerFirstName"]?.ToString() ?? "",
                                    LastName = reader["CustomerLastName"]?.ToString() ?? "",
                                    Phone = reader["CustomerPhone"]?.ToString() ?? "",
                                    Email = reader["CustomerEmail"]?.ToString() ?? "",
                                    CreateAt = reader["PaymentCreateAt"] != DBNull.Value
                                        ? DateTime.Parse(reader["PaymentCreateAt"].ToString())
                                        : (DateTime?)null,
                                    Total = reader["PaymentTotal"] != DBNull.Value
                                        ? Convert.ToSingle(reader["PaymentTotal"])
                                        : (float?)null,
                                    PaymentDetails = new List<PaymentDetailAdmin>()
                                };

                                payments.Add(payment);
                            }

                            // Thêm PaymentDetail nếu có
                            if (reader["ProductId"] != DBNull.Value)
                            {
                                var detail = new PaymentDetailAdmin
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    PaymentId = payment.Id,
                                    Price = Convert.ToInt32(reader["ProductPrice"]),
                                    Quantity = Convert.ToInt32(reader["ProductQuantity"]),
                                    Total = reader["PaymentDetailTotal"] != DBNull.Value
                                        ? Convert.ToSingle(reader["PaymentDetailTotal"])
                                        : (float?)null,
                                    CreateAt = reader["PaymentDetailCreateAt"] != DBNull.Value
                                        ? DateTime.Parse(reader["PaymentDetailCreateAt"].ToString())
                                        : (DateTime?)null
                                };

                                payment.PaymentDetails.Add(detail);
                            }
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi
                    Console.WriteLine("Lỗi khi lấy danh sách payments: " + ex.Message);
                }
            }

            return payments;
        }

        // Thêm Payment mới
        public int AddPayment(PaymentAdmin payment)
        {
            using (var connection = _connect.getConnecttion())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO payment (customerId, firstName, lastName, phone, email, createAt, total) 
                        VALUES (@CustomerId, @FirstName, @LastName, @Phone, @Email, @CreateAt, @Total);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", payment.CustomerId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FirstName", payment.FirstName ?? "");
                        command.Parameters.AddWithValue("@LastName", payment.LastName ?? "");
                        command.Parameters.AddWithValue("@Phone", payment.Phone ?? "");
                        command.Parameters.AddWithValue("@Email", payment.Email ?? "");
                        command.Parameters.AddWithValue("@CreateAt", payment.CreateAt ?? DateTime.Now);
                        command.Parameters.AddWithValue("@Total", payment.Total ?? 0);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi
                    Console.WriteLine("Lỗi khi thêm payment: " + ex.Message);
                    return -1;
                }
            }
        }

        // Thêm PaymentDetail mới
        public void AddPaymentDetail(PaymentDetailAdmin detail)
        {
            using (var connection = _connect.getConnecttion())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO paymentDetail (productId, paymentId, price, quantity, total, createAt) 
                        VALUES (@ProductId, @PaymentId, @Price, @Quantity, @Total, @CreateAt);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", detail.ProductId);
                        command.Parameters.AddWithValue("@PaymentId", detail.PaymentId);
                        command.Parameters.AddWithValue("@Price", detail.Price);
                        command.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        command.Parameters.AddWithValue("@Total", detail.Total ?? 0);
                        command.Parameters.AddWithValue("@CreateAt", detail.CreateAt ?? DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi
                    Console.WriteLine("Lỗi khi thêm chi tiết thanh toán: " + ex.Message);
                }
            }
        }

        // Lấy thông tin Payment theo ID
        public PaymentAdmin GetPaymentById(int id)
        {
            PaymentAdmin payment = null;

            using (var connection = _connect.getConnecttion())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            p.id AS PaymentId,
                            p.customerId AS CustomerId,
                            p.firstName AS CustomerFirstName,
                            p.lastName AS CustomerLastName,
                            p.phone AS CustomerPhone,
                            p.email AS CustomerEmail,
                            p.createAt AS PaymentCreateAt,
                            p.total AS PaymentTotal,
                            pd.productId AS ProductId,
                            pd.price AS ProductPrice,
                            pd.quantity AS ProductQuantity,
                            pd.total AS PaymentDetailTotal,
                            pd.createAt AS PaymentDetailCreateAt
                        FROM 
                            payment p
                        LEFT JOIN 
                            paymentDetail pd ON p.id = pd.paymentId
                        WHERE 
                            p.id = @PaymentId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PaymentId", id);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            if (payment == null)
                            {
                                payment = new PaymentAdmin
                                {
                                    Id = Convert.ToInt32(reader["PaymentId"]),
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : (int?)null,
                                    FirstName = reader["CustomerFirstName"]?.ToString() ?? "",
                                    LastName = reader["CustomerLastName"]?.ToString() ?? "",
                                    Phone = reader["CustomerPhone"]?.ToString() ?? "",
                                    Email = reader["CustomerEmail"]?.ToString() ?? "",
                                    CreateAt = reader["PaymentCreateAt"] != DBNull.Value
                                        ? DateTime.Parse(reader["PaymentCreateAt"].ToString())
                                        : (DateTime?)null,
                                    Total = reader["PaymentTotal"] != DBNull.Value
                                        ? Convert.ToSingle(reader["PaymentTotal"])
                                        : (float?)null,
                                    PaymentDetails = new List<PaymentDetailAdmin>()
                                };
                            }

                            if (reader["ProductId"] != DBNull.Value)
                            {
                                var detail = new PaymentDetailAdmin
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Price = Convert.ToInt32(reader["ProductPrice"]),
                                    Quantity = Convert.ToInt32(reader["ProductQuantity"]),
                                    Total = reader["PaymentDetailTotal"] != DBNull.Value
                                        ? Convert.ToSingle(reader["PaymentDetailTotal"])
                                        : (float?)null,
                                    CreateAt = reader["PaymentDetailCreateAt"] != DBNull.Value
                                        ? DateTime.Parse(reader["PaymentDetailCreateAt"].ToString())
                                        : (DateTime?)null
                                };

                                payment.PaymentDetails.Add(detail);
                            }
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi
                    Console.WriteLine("Lỗi khi lấy payment theo ID: " + ex.Message);
                }
            }

            return payment;
        }
    }
}
