using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;

public static class UrlShortener
{
    public static string GenerateShortKey(string originalUrl)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString().Substring(0, 8);
        }
    }

    public static void StoreUrlInRedis(IDatabase db, string shortKey, string originalUrl)
    {
        db.StringSet(shortKey, originalUrl);
    }
}
