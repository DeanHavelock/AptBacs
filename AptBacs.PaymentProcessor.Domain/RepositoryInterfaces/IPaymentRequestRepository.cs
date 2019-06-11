using AptBacs.PaymentProcessor.Domain.Models;

namespace AptBacs.PaymentProcessor.Domain.RepositoryInterfaces
{
    public interface IPaymentRequestRepository
    {
        int Create(PaymentRequest paymentRequest);
    }
}
