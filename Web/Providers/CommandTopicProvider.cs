using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;

using Commands.Grab;

namespace Web.Providers
{
    [Component]
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public CommandTopicProvider()
        {
            RegisterTopic("GrabOrderCommandTopic", typeof(GrabOrderCommand));
        }
    }
}