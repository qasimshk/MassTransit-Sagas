using System;

namespace CQRS.Models.Payment
{
    public class Payment : IPayment
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }        
    }
}