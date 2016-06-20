using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;

using Commands.OrderProcess;

namespace Service.Event.Providers
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public CommandTopicProvider()
        {
            RegisterTopic("ProcessingOrderCommandTopic", typeof(ProcessingOrderCommand));
        }
    }
}
