using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.Models;
using AptBacs.PaymentProcessor.Domain.Models.Commands;
using AptBacs.PaymentProcessor.Domain.RepositoryInterfaces;

namespace AptBacs.PaymentProcessor.Application.Services
{
    public class ProcessBacsPaymentService : IProcessBacsPaymentService
    {
        private IPaymentRequestRepository _paymentRequestRepository;
        public ProcessBacsPaymentService(IPaymentRequestRepository paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        public void Pay(MakePaymentApplicationCommand makePaymentApplicationCommand)
        {
            //Create Domain Command from Application Command
            var processPaymentDomainCommand = ProcessPaymentDomainCommand.CreateFrom(makePaymentApplicationCommand);

            //Initialize Domain Model
            PaymentRequest paymentRequestDomainModel = new PaymentRequest();

            //Validate Business Logic on Domain Model using Domain Command
            paymentRequestDomainModel.ValidateProcessPaymentRequest(processPaymentDomainCommand);

            //Use third party or internal api to transfer money between accounts
            ///var successfullyProcessedPayments = _transferMoneyService.ProcessPayments(new PaymentsDto(){...})

            //update Domain Model state
            paymentRequestDomainModel.AddProcessPaymentEvent(/*successfullyProcessedPayments*/);

            //Save Domain Model state to Database
            _paymentRequestRepository.Create(paymentRequestDomainModel);

        }
    }
}
