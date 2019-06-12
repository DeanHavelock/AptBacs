using System.Linq;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel;
using AptBacs.PaymentProcessor.Domain.RepositoryInterfaces;

namespace AptBacs.PaymentProcessor.Application.Services
{
    public class BacsPaymentQueryService : IBacsPaymentQueryService
    {
        private IPaymentRequestRepository _paymentRequestRepository;
        public BacsPaymentQueryService(IPaymentRequestRepository paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        public PaymentRequestsForUserReadModel Get(int userId)
        {
            var domainModelResults = _paymentRequestRepository.GetAllForUser(userId);
            var paymentRequestsForUserReadModel = PaymentRequestsForUserReadModel.FromDomainModel(domainModelResults.ToList());
            return paymentRequestsForUserReadModel;
        }

    }
}
