using ECommon.Components;
using ENode.EQueue;
using ENode.Eventing;

using Domain.Grab;
using Domain.OrderProcess;

namespace Service.Command.Providers
{
    [Component]
    public class EventTopicProvider : AbstractTopicProvider<IDomainEvent>
    {
        public EventTopicProvider()
        {
            RegisterTopic("GrabOrderEventTopic", typeof(QiangLockOrderCreateEvent), typeof(WaitQiangUpdateEvent));
            RegisterTopic("ProcessingOrderEventTopic", typeof(NOrderBaseUpdateEvent), typeof(RealSiteInfoUpdateEvent), typeof(OrderFollowUpCreateEvent), typeof(NOrderServiceUpdateEvent), typeof(NOrderServiceCreateEvent), typeof(NOrderServiceLogUpdateEvent), typeof(NOrderServiceLogCreateEvent), typeof(AttentionOrderUpdateEvent), typeof(NCustomerListUpdateEvent), typeof(OrderFollowUpCreateSuccessEvent));
        }
    }
}
