using MassTransit;
using System;

namespace CQRS.Models.Payment
{
    public interface IPaymentReceivedEvent : CorrelatedBy<Guid>
    {
        int StudentId { get; }
        string CourseName { get; }
        DateTime TransactionDate { get; }
        double Amount { get; }        
    }
}