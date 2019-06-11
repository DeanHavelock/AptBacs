using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AptBacs.PaymentProcessor.Domain.Models.Commands
{
    public class ProcessPaymentDomainCommand
    {

        public static ProcessPaymentDomainCommand CreateFrom(MakePaymentApplicationCommand makePaymentApplicationCommand)
        {
            var paymentRequestValueObjects = makePaymentApplicationCommand.paymentRequestValueObjects.ToList().Select(x => new PaymentRequestValueObject() { Code=x.Code, Name=x.Name, Reference=x.Reference, Amount=x.Amount});

            return new ProcessPaymentDomainCommand(paymentRequestValueObjects) { };
        }

        public ProcessPaymentDomainCommand(IEnumerable<PaymentRequestValueObject> paymentRequests)
        {
            PaymentRequests = paymentRequests ?? throw new NullReferenceException("At AptBacs.PaymentProcessor.Domain.Models.Commands.ProcessPaymentCommand ctor paymentRequests is null.");
        }

        public int ProcessPaymentCommandId { get; set; }
        public IEnumerable<PaymentRequestValueObject> PaymentRequests { get;}
    }
}
