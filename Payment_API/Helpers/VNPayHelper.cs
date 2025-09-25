using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Payment_API.Helpers
{
    public class VNPayHelper
    {
        private readonly IConfiguration _configuration;

        public VNPayHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(Guid paymentId, decimal amount, string returnUrl, string orderInfo)
        {
            var vnpayConfig = _configuration.GetSection("VNPAY");
            var vnp_Url = vnpayConfig["BaseUrl"];
            var vnp_TmnCode = vnpayConfig["TmnCode"];
            var vnp_HashSecret = vnpayConfig["HashSecret"];

            // Build VNPay payment URL
            var vnp_Params = new Dictionary<string, string>
                {
                    { "vnp_Version", "2.1.0" },
                    { "vnp_Command", "pay" },
                    { "vnp_TmnCode", vnp_TmnCode },
                    { "vnp_Amount", (amount * 100).ToString(CultureInfo.InvariantCulture).Split('.')[0] }, // Amount in cents
                    { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_IpAddr", Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString() },
                    { "vnp_Locale", "vn" },
                    { "vnp_OrderInfo", orderInfo },
                    { "vnp_OrderType", "other" },
                    { "vnp_ReturnUrl", returnUrl },
                    { "vnp_TxnRef", paymentId.ToString() } // Reference to your internal payment ID
                };

            // Sort dictionary by key
            var sortedParams = new SortedList<string, string>(vnp_Params);

            // Build hash data
            var signData = new StringBuilder();
            foreach (var (key, value) in sortedParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    signData.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
                }
            }

            // Remove last '&'
            if (signData.Length > 0)
            {
                signData.Remove(signData.Length - 1, 1);
            }

            // Create signature
            var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret));
            var hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(signData.ToString()));

            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

            // Add signature to parameters
            sortedParams.Add("vnp_SecureHash", hashString);

            // Build URL
            var queryString = new StringBuilder();
            foreach (var (key, value) in sortedParams)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    queryString.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
                }
            }

            // Remove last '&'
            if (queryString.Length > 0)
            {
                queryString.Remove(queryString.Length - 1, 1);
            }

            return vnp_Url + "?" + queryString;
        }

        public bool ValidateSignature(IDictionary<string, string> vnpayData)
        {
            if (!vnpayData.TryGetValue("vnp_SecureHash", out string inputHash))
            {
                return false;
            }

            var vnp_HashSecret = _configuration["VNPAY:HashSecret"];

            // Remove secure hash from dictionary to validate
            vnpayData.Remove("vnp_SecureHash");
            if (vnpayData.ContainsKey("vnp_SecureHashType"))
            {
                vnpayData.Remove("vnp_SecureHashType");
            }

            // Sort dictionary by key
            var sortedParams = new SortedList<string, string>();
            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sortedParams.Add(key, value);
                }
            }

            // Build hash data
            var signData = new StringBuilder();
            foreach (var (key, value) in sortedParams)
            {
                signData.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            // Remove last '&'
            if (signData.Length > 0)
            {
                signData.Remove(signData.Length - 1, 1);
            }

            // Create signature
            var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret));
            var hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(signData.ToString()));

            var calculatedHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return calculatedHash == inputHash.ToLower();
        }
    }
}
