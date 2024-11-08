using KoiCare.Application.Abtractions.Payments;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace KoiCare.Infrastructure.Dependencies.VnPay
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _config;

        public VnPayService(IOptions<VnPayConfig> config)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public string GeneratePaymentUrl(int orderId, decimal amount, string returnUrl)
        {
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
                { "vnp_Amount", ((long)(amount * 100)).ToString(CultureInfo.InvariantCulture) }, // Amount in VND
                { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", "127.0.0.1" }, // For testing, replace with user's IP if available
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Payment for order {orderId}" },
                { "vnp_OrderType", "billpayment" },
                { "vnp_ReturnUrl", returnUrl },
                { "vnp_TxnRef", orderId.ToString() }
            };

            // Build raw data to sign
            var queryString = VnPayUtils.BuildQueryString(vnPayParams);
            var secureHash = VnPayUtils.GenerateSecureHash(queryString, _config.SecretKey);

            // Append secure hash to the parameters
            vnPayParams["vnp_SecureHash"] = secureHash;

            // Build the full URL
            var paymentUrl = $"{_config.PaymentUrl}?{VnPayUtils.BuildQueryString(vnPayParams)}";
            return paymentUrl;
        }
    }
}
