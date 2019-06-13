using System;
using System.Collections.Generic;
using System.Text;

namespace AptBacs.PaymentProcessor.Domain.Aggregates.ValueObjects
{
    public class PassedInternalChecksReadyToTakePayment
    {
        public string Code { get; set; }
        public string Name { get; internal set; }
        public string Reference { get; internal set; }
        public double Amount { get; internal set; }
    }
}
