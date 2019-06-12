using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand.ValueObjects;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand
{
    public class MakePaymentApplicationCommand
    {
        public MakePaymentApplicationCommand()
        {
            PaymentRequestValueObjects = new List<PaymentRequestValueObject>();
        }

        public string FileName { get; set; }
        public List<PaymentRequestValueObject> PaymentRequestValueObjects { get; set; }
    }
}
