namespace KoiCare.Application.Abtractions.Payments
{
    public interface IVnPayService
    {
        string GeneratePaymentUrl(int orderId, decimal amount, string returnUrl);
    }
}
