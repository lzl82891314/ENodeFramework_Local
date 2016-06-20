using ECommon.Components;
using ENode.Commanding;
using ENode.EQueue;

using Commands.Grab;

namespace Console.Providers
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
