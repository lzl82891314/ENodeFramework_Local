using System;
using ENode.Domain;

namespace Domain.Grab
{
    public class GrabOrder : AggregateRoot<string>
    {
        private string _orderId;
        private long _gjSoufunId;
        private string _gjSoufunName;
        private DateTime _createTime;
        private DateTime _grabOrderDate;

        public GrabOrder(string id, string orderId, long operSoufunId, string operSoufunName, long ownerSoufunId)
            : base(id)
        {
            var nowTime = DateTime.Now;
            ApplyEvents(new QiangLockOrderCreateEvent(this, orderId, operSoufunId, operSoufunName, nowTime), new WaitQiangUpdateEvent(this, orderId, nowTime, 1, operSoufunId, ownerSoufunId));
        }

        private void Handle(QiangLockOrderCreateEvent evnt)
        {
            _orderId = evnt.OrderId;
            _gjSoufunId = evnt.GjSoufunId;
            _gjSoufunName = evnt.GjSoufunName;
            _createTime = evnt.CreateTime;
        }

        private void Handle(WaitQiangUpdateEvent evnt)
        {
            _grabOrderDate = evnt.GrabOrderDate;
        }
    }
}
