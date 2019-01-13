using Automatonymous;
using CQRS.Models.Notification;
using CQRS.Models.Payment;
using MassTransit;
using System;

namespace Sagas.Service.Payment
{
    public class PaymentSaga : MassTransitStateMachine<PaymentSagaStatus>, IPayment
    {
        public PaymentSaga()
        {
            InstanceState(s => s.CurrentState);

            Event(() => PaymentCreate, x =>
            x.CorrelateBy(pay => pay.StudentId.ToString(),
                context => context.Message.StudentId.ToString())
            .SelectId(context => Guid.NewGuid()));

            Event(() => PaymentCompleted, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Event(() => PaymentFailed, x => x.CorrelateById(context =>
                context.Message.CorrelationId));

            Initially(
                When(PaymentCreate)
                .Then(context =>
                {
                    // Will be done by auto mapper                    
                    context.Instance.ReceivedPaymentDate = DateTime.Now;
                    context.Instance.StudentId = context.Data.StudentId;
                    context.Instance.CourseName = context.Data.CourseName;
                    context.Instance.Amount = context.Data.Amount;
                    context.Instance.TransactionDate = context.Data.TransactionDate;

                })
                .If(context => context.Data.Amount == 0, x =>
                    x.Publish(context => new PaymentFailedEvent(context.Instance)))
                    .TransitionTo(ProcessPayment)

                .If(context => context.Data.Amount > 0, x =>                    
                    x.Publish(context => new PaymentReceivedEvent(context.Instance)))
                    .TransitionTo(ProcessPayment)

                // Optional will indicate start
                .ThenAsync(
                    context => Console.Out.WriteLineAsync(
                        $"Payment process initiallise. Id:{context.Instance.CorrelationId}")));

            During(ProcessPayment,
                When(PaymentCompleted)
                    .Then(context => context.Instance.ReceivedPaymentDate = DateTime.Now)
                    .ThenAsync(
                        context => Console.Out.WriteLineAsync(
                            $"Student payment processed. Id: {context.Instance.CorrelationId}")),
                    //.Publish(context => new NotificationReceivedEvent(context.Instance)),

                When(PaymentFailed)
                    .Then(context => context.Instance.ReceivedPaymentDate = DateTime.Now)
                    .ThenAsync(
                        c => Console.Out.WriteLineAsync(
                            $"payment failed for amount: {c.Instance.Amount} Id:{c.Instance.CorrelationId}"))
                    //.Publish(context => new NotificationReceivedEvent(context.Instance))
                    .Finalize());

            SetCompletedWhenFinalized();
        }

        public State ProcessPayment { get; private set; }

        public Event<IPayment> PaymentCreate { get; private set; }
        public Event<IPaymentReceivedEvent> PaymentCompleted { get; private set; }
        public Event<IPaymentFailed> PaymentFailed { get; private set; }

        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
    }

    public class PaymentReceivedEvent : IPaymentReceivedEvent, INotification
    {
        private readonly PaymentSagaStatus _instance;

        public PaymentReceivedEvent(PaymentSagaStatus instance)
        {
            _instance = instance;
        }

        //public Guid CorrelationId => _instance.CorrelationId;
        public int StudentId => _instance.StudentId;
        public string CourseName => _instance.CourseName;
        public DateTime TransactionDate => _instance.TransactionDate;
        public double Amount => _instance.Amount;

        public string Message => $"{_instance.StudentId} is being enrolled in {_instance.CourseName}. Fee paid: £{_instance.Amount}";
        public Guid CorrelationId => _instance.CorrelationId;
    }

    public class PaymentFailedEvent : IPaymentFailed, INotification
    {
        private readonly PaymentSagaStatus _instance;

        public PaymentFailedEvent(PaymentSagaStatus instance)
        {
            _instance = instance;
        }

        public Guid CorrelationId => _instance.CorrelationId;
        public string Message => $"Transaction failed due to insufficent amount: £{_instance.Amount}";
    }


    // NOTE: not required can be accessed directly
    public class NotificationReceivedEvent : INotification
    {
        private readonly PaymentSagaStatus _instance;

        public NotificationReceivedEvent(PaymentSagaStatus instance)
        {
            _instance = instance;
        }

        public Guid CorrelationId => _instance.CorrelationId;

        public string Message =>
            (_instance.Amount > 0) ? $"{_instance.StudentId} is being enrolled in {_instance.CourseName}. Fee paid: £{_instance.Amount}" :
            $"Transaction failed due to insufficent amount: £{_instance.Amount}";
    }
}