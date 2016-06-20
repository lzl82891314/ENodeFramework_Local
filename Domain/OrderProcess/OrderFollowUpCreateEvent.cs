using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class OrderFollowUpCreateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public string FollowDesc { get; private set; }
        public string FollowName { get; private set; }
        public int FollowStageId { get; private set; }
        public int IdentityType { get; private set; }
        public int LogType { get; private set; }
        public int OrderMode { get; private set; }
        public string PosX { get; private set; }
        public string PosY { get; private set; }
        public long SoufunId { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int PageSource { get; private set; }
        public long OpId { get; private set; }
        public string OpUerName { get; private set; }

        private OrderFollowUpCreateEvent() { }

        public OrderFollowUpCreateEvent(ProcessingOrder processModel, string orderId, long ownerSoufunId, int followStageId, string statusName, string followUpDesc, DateTime createTime, int pageSource = 0, int orderMode = 0, long opId = 0, string opUerName = "", int identityType = -1)
            : base(processModel)
        {
            OrderId = orderId;
            SoufunId = ownerSoufunId;
            FollowStageId = followStageId;
            FollowName = statusName;
            FollowDesc = followUpDesc;
            CreateTime = createTime;
            OrderMode = orderMode;
            PosX = "0.0";
            PosY = "0.0";
            IdentityType = identityType;
            LogType = -1;
            PageSource = pageSource;
            OpId = opId;
            OpUerName = opUerName;
        }
    }
}
