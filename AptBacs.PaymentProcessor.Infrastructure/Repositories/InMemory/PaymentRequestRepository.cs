using AptBacs.PaymentProcessor.Domain.Models;
using AptBacs.PaymentProcessor.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace AptBacs.PaymentProcessor.Infrastructure.Repositories.InMemory
{
    //In-Memory Repository, Static Values, Singleton for In-Memory Database Storage.
    public class PaymentRequestRepository : IPaymentRequestRepository
    {
        private static List<PaymentRequest> _paymentRequests;

        public PaymentRequestRepository()
        {
            _paymentRequests = new List<PaymentRequest>();
        }

        public int Create(PaymentRequest paymentRequest)
        {
            //Manual Unique Id Management (EF handles this + table migrations + persistent disk storage)
            int paymentRequestId = _paymentRequests.Count + 1;
            paymentRequest.PaymentRequestId = paymentRequestId;

            int runningTotal = 0;
            foreach (var successfulPayment in paymentRequest.SuccessfulPayments)
            {
                
                var newIdentityValue = _paymentRequests.Sum(p => p.SuccessfulPayments.Count()) + 1 + runningTotal;
                successfulPayment.SuccessfulPaymentId = newIdentityValue;
                successfulPayment.PaymentRequestId = paymentRequestId;
                runningTotal++;
            }
            runningTotal = 0;
            foreach (var failedPayment in paymentRequest.FailedPayments)
            {
                var newIdentityValue = _paymentRequests.Sum(p => p.FailedPayments.Count()) + 1 + runningTotal;
                failedPayment.FailedPaymentId = newIdentityValue;
                failedPayment.PaymentRequestId = paymentRequestId;
            }
            runningTotal = 0;
            foreach (var fraudCheckFlaggedOnHoldManualInterventionRequiredPayment in paymentRequest.FraudCheckFlaggedOnHoldManualInterventionRequiredPayments)
            {
                var newIdentityValue = _paymentRequests.Sum(p => p.FraudCheckFlaggedOnHoldManualInterventionRequiredPayments.Count()) + 1 + runningTotal;
                fraudCheckFlaggedOnHoldManualInterventionRequiredPayment.FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentId = newIdentityValue;
                fraudCheckFlaggedOnHoldManualInterventionRequiredPayment.PaymentRequestId = paymentRequestId;
            }

            //Save Record to Memory:
            _paymentRequests.Add(paymentRequest);

            //Return Id of newly created aggregate resource
            return paymentRequest.PaymentRequestId;
        }

        public IEnumerable<PaymentRequest> GetAllForUser(int userId)
        {
            return _paymentRequests;//.Where(x=>x.UserId == userId); (return all for demo..)
        }
    }
}
