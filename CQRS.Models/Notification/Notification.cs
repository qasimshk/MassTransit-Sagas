using System;

namespace CQRS.Models.Notification
{
    public class Notification : INotification
    {
        public string Message { get; set; }
        public Guid CorrelationId { get; set; }
    }
}