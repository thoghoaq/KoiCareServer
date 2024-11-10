namespace KoiCare.Infrastructure.Dependencies.VnPay
{
    public class VnPayConfig
    {
        public string TerminalId { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string PaymentUrl { get; set; } = ""; // Base URL for VNPAY
        public string ReturnUrl { get; set; } = ""; // URL to return after payment
    }
}
