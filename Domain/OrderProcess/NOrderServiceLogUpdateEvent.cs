using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class NOrderServiceLogUpdateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public int ServiceId { get; private set; }
        public DateTime OldStopTime { get; private set; }
        public DateTime NewStopTime { get; private set; }
        public int IsCurrent { get; private set; }

        private NOrderServiceLogUpdateEvent() { }

        public NOrderServiceLogUpdateEvent(ProcessingOrder processModel, string orderId, int serviceId, DateTime newStopTime)
            : base(processModel)
        {
            OrderId = orderId;
            ServiceId = serviceId;
            NewStopTime = newStopTime;
            IsCurrent = 0;
            OldStopTime = DateTime.Parse("1900-01-01");
        }
    }
}
