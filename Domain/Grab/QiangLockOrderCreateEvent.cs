using System;
using ENode.Eventing;

namespace Domain.Grab
{
    public class QiangLockOrderCreateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public long GjSoufunId { get; private set; }
        public string GjSoufunName { get; private set; }
        public DateTime CreateTime { get; private set; }

        private QiangLockOrderCreateEvent() { }

        public QiangLockOrderCreateEvent(GrabOrder grabModel, string orderId, long gjSoufunId, string gjSoufunName, DateTime createTime)
            : base(grabModel)
        {
            OrderId = orderId;
            GjSoufunId = gjSoufunId;
            GjSoufunName = gjSoufunName;
            CreateTime = createTime;
        }
    }
}
