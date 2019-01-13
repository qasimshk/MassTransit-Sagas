using Automatonymous;
using CQRS.Models.Notification;
using CQRS.Models.Payment;
using System;

namespace Sagas.Service.Payment
{
    public class PaymentSagaStatus : SagaStateMachineInstance, IPayment //, INotification
    {
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }
        public DateTime ReceivedPaymentDate { get; set; }

        // IPayment
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }

        // INotification
        //public string Message { get; set; }
    }    
}