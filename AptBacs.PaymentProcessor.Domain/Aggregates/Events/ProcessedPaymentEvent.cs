using AptBacs.PaymentProcessor.Domain.Aggregates.Models;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.Models.Events
{
    public class ProcessedPaymentEvent
    {
        public ProcessedPaymentEvent(IEnumerable<SuccessfulPayment> successfulPayments, IEnumerable<FailedPayment> failedPayments, IEnumerable<FraudCheckFlaggedOnHoldManualInterventionRequiredPayment> fraudCheckFlaggedOnHoldManualInterventionRequiredPayments)
        {
            SuccessfulPayments = successfulPayments;
            FailedPayments = failedPayments;
            FraudCheckFlaggedOnHoldManualInterventionRequiredPayments = fraudCheckFlaggedOnHoldManualInterventionRequiredPayments;
        }

        public IEnumerable<SuccessfulPayment> SuccessfulPayments { get; }
        public IEnumerable<FailedPayment> FailedPayments { get; }
        public IEnumerable<FraudCheckFlaggedOnHoldManualInterventionRequiredPayment> FraudCheckFlaggedOnHoldManualInterventionRequiredPayments { get; }
    }
}
