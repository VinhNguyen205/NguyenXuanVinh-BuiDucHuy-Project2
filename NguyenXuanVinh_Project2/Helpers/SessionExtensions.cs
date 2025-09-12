using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace NguyenXuanVinh_Project2.Helpers
{
    public static class SessionExtensions
    {
        // Lưu object vào session dưới dạng JSON
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Lấy object từ session và convert về kiểu T
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
