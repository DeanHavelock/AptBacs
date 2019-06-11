using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;

namespace AptBacs.PaymentProcessor.Domain.ApplicationInterfaces
{
    public interface IProcessBacsPaymentService
    {
        void Pay(MakePaymentApplicationCommand makePaymentCommand);
    }
}
