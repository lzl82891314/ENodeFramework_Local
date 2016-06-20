using System;
using ENode.Eventing;

namespace Domain.Grab
{
    public class WaitQiangUpdateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public DateTime GrabOrderDate { get; private set; }
        public int IsDel { get; private set; }
        public long OperSoufunId { get; private set; }
        public long OwnerSoufunId { get; private set; }

        private WaitQiangUpdateEvent() { }

        public WaitQiangUpdateEvent(GrabOrder grabModel, string orderId, DateTime grabOrderDate, int isDel, long operSoufunId, long ownerSoufunId)
            : base(grabModel)
        {
            OrderId = orderId;
            GrabOrderDate = grabOrderDate;
            IsDel = isDel;
            OperSoufunId = operSoufunId;
            OwnerSoufunId = ownerSoufunId;
        }
    }
}
