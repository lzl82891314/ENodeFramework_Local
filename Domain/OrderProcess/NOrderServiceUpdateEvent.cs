using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class NOrderServiceUpdateEvent : DomainEvent<string>
    {
        public string OrderId { get; private set; }
        public int CompanyId { get; private set; }
        public string CompanyName { get; private set; }
        public int GroupId { get; private set; }
        public string GroupName { get; private set; }
        public int IsServiceUser { get; private set; }
        public string Phone { get; private set; }
        public long SoufunId { get; private set; }
        public string SoufunName { get; private set; }
        public string TrueName { get; private set; }
        public int FunctionId { get; private set; }
        public int ServiceId { get; private set; }
        public string ServiceName { get; private set; }
        public DateTime UpdateTime { get; private set; }

        private NOrderServiceUpdateEvent() { }

        public NOrderServiceUpdateEvent(ProcessingOrder processModel, string orderId, int companyId, string companyName, int groupId, string groupName, int isServiceUser, string phone, long soufunId, string soufunName, string trueName, int functionId, int serviceId, string serviceName, DateTime updateTime)
            : base(processModel)
        {
            OrderId = orderId;
            CompanyId = companyId;
            CompanyName = companyName;
            GroupId = groupId;
            GroupName = groupName;
            IsServiceUser = isServiceUser;
            Phone = phone;
            SoufunId = soufunId;
            SoufunName = soufunName;
            TrueName = trueName;
            FunctionId = functionId;
            ServiceId = serviceId;
            ServiceName = serviceName;
            UpdateTime = updateTime;
        }
    }
}
