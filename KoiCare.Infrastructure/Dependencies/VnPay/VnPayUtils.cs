using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KoiCare.Infrastructure.Dependencies.VnPay
{
    public static class VnPayUtils
    {
        public static string GenerateSecureHash(string input, string secretKey)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public static string BuildQueryString(SortedDictionary<string, string> parameters)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }
            return query.ToString();
        }
    }
}
