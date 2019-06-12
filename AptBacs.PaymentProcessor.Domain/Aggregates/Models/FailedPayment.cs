namespace AptBacs.PaymentProcessor.Domain.Aggregates.Models
{
    public class FailedPayment
    {
        public int FailedPaymentId { get; set; }
        public int PaymentRequestId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
    }
}
