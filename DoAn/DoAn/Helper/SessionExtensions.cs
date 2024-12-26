using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DoAn.Helper
{
    public static class SessionExtensions
    {
        // Phương thức lưu đối tượng vào session
        public static void Set<T>(this ISession session, string key, T value)
        {
            // Serialize đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(value);
            session.SetString(key, jsonData);
        }

        // Phương thức lấy đối tượng từ session
        public static T Get<T>(this ISession session, string key)
        {
            // Lấy chuỗi JSON từ session
            var jsonData = session.GetString(key);
            return jsonData == null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
