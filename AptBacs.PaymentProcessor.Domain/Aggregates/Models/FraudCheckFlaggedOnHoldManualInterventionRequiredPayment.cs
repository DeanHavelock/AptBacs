﻿namespace AptBacs.PaymentProcessor.Domain.Aggregates.Models
{
    public class FraudCheckFlaggedOnHoldManualInterventionRequiredPayment
    {
        public int FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentId { get; set; }
        public int PaymentRequestId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
    }
}
