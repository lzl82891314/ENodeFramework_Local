using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class AttentionOrderUpdateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public long SoufunId { get; private set; }

        private AttentionOrderUpdateEvent() { }

        public AttentionOrderUpdateEvent(ProcessingOrder processModel, string orderId, long soufunId)
            : base(processModel)
        {
            OrderId = orderId;
            SoufunId = soufunId;
        }
    }
}
