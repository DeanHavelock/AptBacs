using AptBacs.PaymentProcessor.Domain.Aggregates.Models;
using AptBacs.PaymentProcessor.Domain.Models.Commands;
using AptBacs.PaymentProcessor.Domain.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AptBacs.PaymentProcessor.Domain.Models
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {
            SuccessfulPayments = new List<SuccessfulPayment>();
            FailedPayments = new List<FailedPayment>();
            FraudCheckFlaggedOnHoldManualInterventionRequiredPayments = new List<FraudCheckFlaggedOnHoldManualInterventionRequiredPayment>();
            _processPaymentDomainCommand = new ProcessPaymentDomainCommand("", new List<PaymentRequestValueObject>() { });
            Timestamp = DateTime.UtcNow;
        }

        private ProcessPaymentDomainCommand _processPaymentDomainCommand { get; set; }
        ///For Domain Event Publish
        //private ProcessedPaymentEvent _processedPaymentEvent { get { return new ProcessedPaymentEvent(SuccessfulPayments, FailedPayments, FraudCheckFlaggedOnHoldManualInterventionRequiredPayments); } }

        public int PaymentId { get; set; }

        public DateTime Timestamp { get; set; }
        public string FileName { get; set; }


        public List<SuccessfulPayment> SuccessfulPayments { get; set; }
        public List<FailedPayment> FailedPayments { get; set; }
        public List<FraudCheckFlaggedOnHoldManualInterventionRequiredPayment> FraudCheckFlaggedOnHoldManualInterventionRequiredPayments { get; set; }


        /// ToDo: In the spec we are asked to store these values in the Database, I would recommend refactoring to the following: (only when the calculation became a performance issue would we create a read model projection), these totals should be calculated at runtime instead and made available on a DTO through the Api, I would refactor these fields to private with a public method on the domain object to calculate these values for populating a DTO and returning through our api when requested.
        public double SuccessfullPaymentsTotal { get { return SuccessfulPayments.Sum(x => x.Amount); } set { SuccessfullPaymentsTotal = value; } }
        public double FailedPaymentsTotal { get { return FailedPayments.Sum(x => x.Amount); } set { SuccessfullPaymentsTotal = value; } }
        public double FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentsTotal { get { return FraudCheckFlaggedOnHoldManualInterventionRequiredPayments.Sum(x => x.Amount); } set { SuccessfullPaymentsTotal = value; } }
        public double TotalValueOfPaymentsRequested { get { return SuccessfullPaymentsTotal + FailedPaymentsTotal + FraudCheckFlaggedOnHoldManualInterventionRequiredPaymentsTotal; } set { TotalValueOfPaymentsRequested = value; } }

        public void ProcessPaymentRequest(ProcessPaymentDomainCommand processPaymentDomainCommand)
        {
            var passingValidationRules = CheckPassingValidationRules(processPaymentDomainCommand);
            if (!passingValidationRules)
                throw new Exception("AptBacs.PaymentProcessor.Domain.Models.PaymentRequest.AddProcessPaymentRequest() CheckPassingValidationRules(processPaymentCommand) -> failed validation checks");

            FileName = processPaymentDomainCommand.FileName ?? throw new Exception("AptBacs.PaymentProcessor.Domain.Models.PaymentRequest.AddProcessPaymentRequest() -> received FileName was null");
            _processPaymentDomainCommand = processPaymentDomainCommand;
        }

        private bool CheckPassingValidationRules(ProcessPaymentDomainCommand processPaymentCommand)
        {
            var hasPassedValidationCheckResult = false;
            if (processPaymentCommand == null || processPaymentCommand.PaymentRequests == null || !processPaymentCommand.PaymentRequests.ToList().Any())
                return hasPassedValidationCheckResult;

            //checking validation logic:
            // - Minimum amount is £1.00.
            // - Maximum amount is £20,000,000.00.
            if (processPaymentCommand.PaymentRequests.ToList().Any(request => request.Amount < 1 || request.Amount > 20000000.00))
                return hasPassedValidationCheckResult;

            hasPassedValidationCheckResult = true;
            return hasPassedValidationCheckResult;
        }
        public void AddProcessPaymentEvent(/*ProcessedPayments processedPayments*/)
        {
            //from processedPayments assign: successfulPayments, FailedPayments and FraudCheckFlaggedOnHoldManualInterventionRequiredPayments for persistance:
            SuccessfulPayments = _processPaymentDomainCommand.PaymentRequests.Select(x => new SuccessfulPayment() { Code=x.Code, Name=x.Name, Reference=x.Reference, Amount=x.Amount }).ToList();

            //Raise Domain Event ProcessedPayments: (for any interested other bounded contexts that might have the requirement to build read model projections from this event (surfaced through a domain event handler with an integration event publish).
            //RaiseDomainEvent(_processedPaymentEvent);
        }
    }
}
