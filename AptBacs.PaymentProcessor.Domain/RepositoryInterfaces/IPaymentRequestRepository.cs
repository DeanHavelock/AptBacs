using AptBacs.PaymentProcessor.Domain.Models;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.RepositoryInterfaces
{
    public interface IPaymentRequestRepository
    {
        int Create(PaymentRequest paymentRequest);
        IEnumerable<PaymentRequest> GetAllForUser(int userId);
    }
}
