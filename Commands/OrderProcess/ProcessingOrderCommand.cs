using System;
using ENode.Commanding;

namespace Commands.OrderProcess
{
    public class ProcessingOrderCommand : Command
    {
        public string OrderId { get; private set; }
        public long OperSoufunId { get; private set; }
        public long OwnerSoufunId { get; private set; }

        private ProcessingOrderCommand() { }

        public ProcessingOrderCommand(string id, string orderId, long operSoufunId, long ownerSoufunId)
            : base(id)
        {
            OrderId = orderId;
            OperSoufunId = operSoufunId;
            OwnerSoufunId = ownerSoufunId;
        }
    }
}
