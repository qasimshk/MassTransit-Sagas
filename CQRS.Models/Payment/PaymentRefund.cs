using System;

namespace CQRS.Models.Payment
{
    public class PaymentRefund : IPaymentRefund
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
