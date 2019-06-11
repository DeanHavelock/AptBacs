namespace AptBacs.PaymentProcessor.Domain.Models.ValueObjects
{
    public class PaymentRequestValueObject
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public double Amount { get; set; }
    }
}
