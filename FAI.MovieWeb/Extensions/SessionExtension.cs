namespace FAI.MovieWeb.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            session.SetString(key, serializedValue);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
