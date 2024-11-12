using KoiCare.Application.Abtractions.Payments;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace KoiCare.Infrastructure.Dependencies.VnPay
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _config;

        public VnPayService(IOptions<VnPayConfig> config)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public string GeneratePaymentUrl(int orderId, decimal amount, string? returnUrl = null)
        {
            returnUrl ??= _config.ReturnUrl;
            // Validate required fields
            if (string.IsNullOrWhiteSpace(_config.TerminalId)) throw new ArgumentNullException(nameof(_config.TerminalId), "Terminal ID is required.");
            if (string.IsNullOrWhiteSpace(_config.SecretKey)) throw new ArgumentNullException(nameof(_config.SecretKey), "Secret Key is required.");
            if (string.IsNullOrWhiteSpace(_config.PaymentUrl)) throw new ArgumentNullException(nameof(_config.PaymentUrl), "Payment URL is required.");
            if (string.IsNullOrWhiteSpace(returnUrl)) throw new ArgumentNullException(nameof(returnUrl), "Return URL is required.");

            var vnPayParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _config.TerminalId },
                { "vnp_Amount", (amount * 100).ToString() }, // Amount in VND
                { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", "127.0.0.1" }, // For testing, replace with user's IP if available
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Payment for order {orderId}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", returnUrl },
                { "vnp_TxnRef", orderId.ToString() }
            };

            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vnPayParams)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            // Remove trailing "&" if present
            if (data.Length > 0)
            {
                data.Length -= 1; // Remove last '&'
            }

            // Create the complete payment URL and secure hash
            string paymentUrl = $"{_config.PaymentUrl}?{data}";
            string vnp_SecureHash = VnPayUtils.GenerateSecureHash(data.ToString(), _config.SecretKey);
            paymentUrl += "&vnp_SecureHash=" + vnp_SecureHash;

            return paymentUrl;
        }
    }
}
