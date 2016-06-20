using System;
using ENode.Commanding;

namespace Commands.Grab
{
    public class GrabOrderCommand : Command
    {
        public string OrderId { get; private set; }
        public long OperSoufunId { get; private set; }
        public string OperSoufunName { get; private set; }
        public long OwnerSoufunId { get; private set; }

        private GrabOrderCommand() { }

        public GrabOrderCommand(string id, string orderId, long operSoufunId, string operSoufunName, long ownerSoufunId)
            : base(id)
        {
            OrderId = orderId;
            OperSoufunId = operSoufunId;
            OperSoufunName = operSoufunName;
            OwnerSoufunId = ownerSoufunId;
        }
    }
}
