using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces
{
    public interface IBacsPaymentQueryService
    {
        PaymentRequestsForUserReadModel Get(int userId);
    }
}
