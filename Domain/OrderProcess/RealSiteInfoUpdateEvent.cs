using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class RealSiteInfoUpdateEvent :  DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public int StatusId { get; private set; }
        public string StatusName { get; private set; }
        public int OrderType { get; private set; }

        private RealSiteInfoUpdateEvent() { }

        public RealSiteInfoUpdateEvent(ProcessingOrder processModel, string orderId, int statusId, string statusName, int orderType)
            : base(processModel)
        {
            OrderId = orderId;
            StatusId = statusId;
            StatusName = statusName;
            OrderType = orderType;
        }
    }
}
