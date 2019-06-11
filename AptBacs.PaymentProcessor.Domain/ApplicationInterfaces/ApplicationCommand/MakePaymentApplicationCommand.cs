using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand.ValueObjects;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand
{
    public class MakePaymentApplicationCommand
    {
        public MakePaymentApplicationCommand()
        {
            paymentRequestValueObjects = new List<PaymentRequestValueObject>();
        }

        public IEnumerable<PaymentRequestValueObject> paymentRequestValueObjects { get; set; }
    }
}
