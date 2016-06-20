using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class NOrderServiceLogCreateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public int ServiceId { get; private set; }
        public long SoufunId { get; private set; }
        public string SoufunName { get; private set; }
        public string Cause { get; private set; }
        public string Remark { get; private set; }
        public int IsCurrent { get; private set; }
        public DateTime CreateTime { get; private set; }

        private NOrderServiceLogCreateEvent() { }

        public NOrderServiceLogCreateEvent(ProcessingOrder processModel, string orderId, int serviceId, long soufunId, string soufunName, string causeText, string remark, DateTime createTime)
            : base(processModel)
        {
            OrderId = orderId;
            ServiceId = serviceId;
            SoufunId = soufunId;
            SoufunName = soufunName;
            Cause = causeText;
            Remark = remark;
            IsCurrent = 1;
            CreateTime = createTime;
        }
    }
}
