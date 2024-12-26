using DoAn.Areas.Admin.Database;
using DoAn.Areas.Admin.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DoAn.Areas.Admin.DAL
{
    public class HomeAdminDAL
    {
        private readonly DBconnect _context = new DBconnect();

        // Lấy tổng số thanh toán
        public int GetTotalPayments()
        {
            int count = 0;

            try
            {
                _context.openConnection();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Payment", _context.getConnecttion()))
                {
                    count = (int)command.ExecuteScalar();
                }
            }
            finally
            {
                _context.closeConnection();
            }

            return count;
        }

        // Lấy tổng số sản phẩm
        public int GetTotalProducts()
        {
            int count = 0;

            try
            {
                _context.openConnection();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Product", _context.getConnecttion()))
                {
                    count = (int)command.ExecuteScalar();
                }
            }
            finally
            {
                _context.closeConnection();
            }

            return count;
        }

        // Lấy tổng số tài khoản
        public int GetTotalCustomers()
        {
            int count = 0;

            try
            {
                _context.openConnection();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM Customer", _context.getConnecttion()))
                {
                    count = (int)command.ExecuteScalar();
                }
            }
            finally
            {
                _context.closeConnection();
            }

            return count;
        }

        // Lấy tổng doanh thu theo tháng và năm
        public decimal GetMonthlyRevenue(int year, int month)
        {
            decimal totalRevenue = 0;

            try
            {
                _context.openConnection();

                string query = @"
                    SELECT 
                        SUM(pd.total) AS TotalRevenue
                    FROM 
                        payment p
                    INNER JOIN 
                        paymentDetail pd ON p.id = pd.paymentId
                    WHERE 
                        YEAR(p.createAt) = @Year AND MONTH(p.createAt) = @Month
                ";

                using (var command = new SqlCommand(query, _context.getConnecttion()))
                {
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);

                    // Thực thi câu lệnh và lấy kết quả
                    var result = command.ExecuteScalar();

                    // Kiểm tra và gán giá trị
                    totalRevenue = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
            finally
            {
                _context.closeConnection();
            }

            return totalRevenue;
        }
    }
}
