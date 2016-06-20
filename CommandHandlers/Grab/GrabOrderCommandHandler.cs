using ECommon.Components;
using ENode.Commanding;
using ENode.Infrastructure;

using Commands.Grab;
using Domain.Grab;

namespace CommandHandlers.Grab
{
    [Component]
    public class GrabOrderCommandHandler : ICommandHandler<GrabOrderCommand>
    {
        private readonly ILockService _lockService;
        public GrabOrderCommandHandler(ILockService lockService)
        {
            _lockService = lockService;
        }

        public void Handle(ICommandContext context, GrabOrderCommand command)
        {
            _lockService.ExecuteInLock(typeof(GrabOrder).Name, () =>
            {
                GrabOrderLockService.OrderGrabValidate(command.OrderId);
                context.Add(new GrabOrder(command.AggregateRootId, command.OrderId, command.OperSoufunId, command.OperSoufunName, command.OwnerSoufunId));
            });
        }
    }
}
