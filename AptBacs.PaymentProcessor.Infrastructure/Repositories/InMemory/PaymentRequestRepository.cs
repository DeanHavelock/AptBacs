using AptBacs.PaymentProcessor.Domain.Models;
using AptBacs.PaymentProcessor.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace AptBacs.PaymentProcessor.Infrastructure.Repositories.InMemory
{
    public class PaymentRequestRepository : IPaymentRequestRepository
    {
        private static List<PaymentRequest> _paymentRequests;

        public PaymentRequestRepository()
        {
            _paymentRequests = new List<PaymentRequest>();
        }

        public int Create(PaymentRequest paymentRequest)
        {
            //Manual Unique Id Management
            paymentRequest.PaymentId = _paymentRequests.Count + 1;
            foreach(var successfulPayment in paymentRequest.SuccessfulPayments)
            {
                var newIdentityValue = _paymentRequests.Select(p => p.SuccessfulPayments).Count() + 1;
                successfulPayment.SuccessfulPaymentId = newIdentityValue;
            }
            foreach (var failedPayment in paymentRequest.FailedPayments)
            {
                var newIdentityValue = _paymentRequests.Select(p => p.FailedPayments).Count() + 1;
                failedPayment.FailedPaymentId = newIdentityValue;
            }
            foreach (var fraudCheckFlaggedOnHoldManualInterventionRequiredPayment in paymentRequest.FraudCheckFlaggedOnHoldManualInterventionRequiredPayments)
            {
                var newIdentityValue = _paymentRequests.Select(p => p.FraudCheckFlaggedOnHoldManualInterventionRequiredPayments).Count() + 1;
                fraudCheckFlaggedOnHoldManualInterventionRequiredPayment.FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentId = newIdentityValue;
            }

            //Save Record to Memory:
            _paymentRequests.Add(paymentRequest);

            //Return Id of newly created aggregate resource
            return paymentRequest.PaymentId;
        }

    }
}
