using DoAn.Areas.Admin.Database;
using DoAn.Areas.Admin.Models;
using DoAn.Models;
using Microsoft.Data.SqlClient;
using Syncfusion.EJ2.Navigations;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DoAn.Areas.Admin.DAL
{
    public class MenuAdminDAL
    {
        DBconnect dBconnect = new DBconnect();

        public List<MenuAdmin> GetAllMenus()
        {
            var menus = new List<MenuAdmin>();

            try
            {
                using (var connection = dBconnect.getConnecttion())
                {
                    connection.Open();
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;

                        string query = @"
                    SELECT 
                        id, 
                        title, 
                        menuUrl, 
                        parentId, 
                        menuIndex, 
                        isVisible
                    FROM menu
                    ORDER BY parentId, menuIndex";

                        command.CommandText = query;

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var menu = new MenuAdmin
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Title = reader["title"]?.ToString() ?? string.Empty,
                                    MenuUrl = reader["menuUrl"]?.ToString() ?? string.Empty,
                                    ParentId = reader["parentId"] as int?,
                                    MenuIndex = Convert.ToInt32(reader["menuIndex"]),
                                    isVisible = Convert.ToBoolean(reader["isVisible"])
                                };

                                menus.Add(menu);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi tại đây (ghi log hoặc throw lại exception)
                throw new Exception("Error fetching menus", ex);
            }

            return menus;
        }


    }
}
