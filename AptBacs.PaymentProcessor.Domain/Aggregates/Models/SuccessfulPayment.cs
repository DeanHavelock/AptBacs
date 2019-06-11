namespace AptBacs.PaymentProcessor.Domain.Aggregates.Models
{
    public class SuccessfulPayment
    {
        public int SuccessfulPaymentId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
    }
}
