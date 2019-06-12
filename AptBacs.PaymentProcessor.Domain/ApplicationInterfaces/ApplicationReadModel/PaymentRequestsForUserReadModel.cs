using AptBacs.PaymentProcessor.Domain.Models;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel
{
    public class PaymentRequestsForUserReadModel
    {
        public static PaymentRequestsForUserReadModel FromDomainModel(List<PaymentRequest> paymentRequests)
        {
            var paymentRequestsForUserReadModel = new PaymentRequestsForUserReadModel();
            foreach (var paymentRequest in paymentRequests)
            {
                var paymentRequestForUser = new PaymentRequestForUserReadModel()
                {
                    PaymentRequestId = paymentRequest.PaymentRequestId,
                    FileName = paymentRequest.FileName,
                    Timestamp = paymentRequest.Timestamp,

                    SuccessfulPayments = paymentRequest.SuccessfulPayments,
                    SuccessfullPaymentsTotal = paymentRequest.SuccessfullPaymentsTotal,

                    FailedPayments = paymentRequest.FailedPayments,
                    FailedPaymentsTotal = paymentRequest.FailedPaymentsTotal,

                    FraudCheckFlaggedOnHoldManualInterventionRequiredPayments = paymentRequest.FraudCheckFlaggedOnHoldManualInterventionRequiredPayments,
                    FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentsTotal = paymentRequest.FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentsTotal,

                    TotalValueOfPaymentsRequested = paymentRequest.TotalValueOfPaymentsRequested
                };
                paymentRequestsForUserReadModel.PaymentRequestsForUser.Add(paymentRequestForUser);
            }
            return paymentRequestsForUserReadModel;
        }
        
        public PaymentRequestsForUserReadModel()
        {
            PaymentRequestsForUser = new List<PaymentRequestForUserReadModel>();   
        }

        public List<PaymentRequestForUserReadModel> PaymentRequestsForUser { get; set; }
    }
}
