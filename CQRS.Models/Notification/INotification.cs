using MassTransit;
using System;

namespace CQRS.Models.Notification
{
    public interface INotification : CorrelatedBy<Guid>
    {
        string Message { get; }
    }
}
