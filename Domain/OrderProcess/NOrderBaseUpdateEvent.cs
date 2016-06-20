using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class NOrderBaseUpdateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public int StatusId { get; private set; }
        public string StatusName { get; private set; }
        public int UserRank { get; private set; }
        public int OrderType { get; private set; }
        public DateTime GrabOrderDate { get; private set; }

        private NOrderBaseUpdateEvent() { }

        public NOrderBaseUpdateEvent(ProcessingOrder processModel, string orderId, int statusId, string statusName, int userRank, int orderType, DateTime grabOrderDate)
            : base(processModel)
        {
            OrderId = orderId;
            StatusId = statusId;
            StatusName = statusName;
            UserRank = userRank;
            OrderType = orderType;
            GrabOrderDate = grabOrderDate;
        }
    }
}
