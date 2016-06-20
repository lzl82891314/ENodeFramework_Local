using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;

using Commands.OrderProcess;
using Domain.OrderProcess;

namespace CommandHandlers.OrderProcess
{
    [Component]
    public class ProcessingOrderCommandHandler : ICommandHandler<ProcessingOrderCommand>
    {
        public void Handle(ICommandContext context, ProcessingOrderCommand command)
        {
            context.Add(new ProcessingOrder(command.AggregateRootId, command.OrderId, command.OperSoufunId, command.OwnerSoufunId));
        }
    }
}
