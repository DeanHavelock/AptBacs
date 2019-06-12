using AptBacs.PaymentProcessor.Domain.Aggregates.Models;
using System;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel
{
    public class PaymentRequestForUserReadModel
    {
        public int PaymentRequestId { get; set; }
        public string FileName { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SuccessfulPayment> SuccessfulPayments { get; set; }
        public double SuccessfullPaymentsTotal { get; set; }
        public List<FailedPayment> FailedPayments { get; set; }
        public double FailedPaymentsTotal { get; set; }
        public List<FraudCheckFlaggedOnHoldManualInterventionRequiredPayment> FraudCheckFlaggedOnHoldManualInterventionRequiredPayments { get; set; }
        public double FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentsTotal { get; set; }
        public double TotalValueOfPaymentsRequested { get; set; }
    }
}
