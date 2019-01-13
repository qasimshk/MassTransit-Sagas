using MassTransit;
using System;

namespace CQRS.Models.Payment
{
    public interface IPaymentRefund : CorrelatedBy<Guid>
    {
        int StudentId { get; }
        string CourseName { get; }
        DateTime TransactionDate { get; }
        double Amount { get; }
    }
}
