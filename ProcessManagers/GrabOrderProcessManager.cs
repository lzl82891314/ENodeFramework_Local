using System.Threading.Tasks;

using ECommon.Components;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Infrastructure;
using ENode.Commanding;

using Domain.Grab;
using Commands.OrderProcess;

namespace ProcessManagers
{
    [Component]
    public class GrabOrderProcessManager : IMessageHandler<WaitQiangUpdateEvent>
    {
        private ICommandService _commandService;

        public GrabOrderProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public Task<AsyncTaskResult> HandleAsync(WaitQiangUpdateEvent message)
        {
            return _commandService.SendAsync(new ProcessingOrderCommand(ObjectId.GenerateNewStringId(), message.OrderId, message.OperSoufunId, message.OwnerSoufunId));
        }
    }
}
