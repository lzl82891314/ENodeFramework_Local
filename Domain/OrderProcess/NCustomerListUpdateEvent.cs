using System;
using ENode.Eventing;

namespace Domain.OrderProcess
{
    public class NCustomerListUpdateEvent : DomainEvent<string>
    {
        #region 条件
        public string OrderId { get; private set; }
        public int FunctionId { get; private set; }
        #endregion

        #region 修改字段
        public DateTime EnterSystemTime { get; private set; }
        public int UserRank { get; private set; }
        public int StatusId { get; private set; }
        public string StatusName { get; private set; }
        public int OrderType { get; private set; }
        public DateTime LastFollowUpTime { get; private set; }
        public int CompanyId { get; private set; }
        public string CompanyName { get; private set; }
        public int GroupId { get; private set; }
        public string GroupName { get; private set; }
        public long SoufunId { get; private set; }
        public string SoufunName { get; private set; }
        public string TrueName { get; private set; }
        public long LastFollowUpSoufunId { get; private set; }
        public string LastFollowUpTrueName { get; private set; }
        #endregion

        private NCustomerListUpdateEvent() { }

        public NCustomerListUpdateEvent(ProcessingOrder processModel, string orderId, int functionId, long soufunId, string soufunName, string trueName, int groupId, string groupName, int companyId, string companyName, int orderType, int statusId, string statusName, int userRank, DateTime grabOrderDate, DateTime createTime, long opId, string opUerName)
            : base(processModel)
        {
            OrderId = orderId;
            FunctionId = functionId;
            SoufunId = soufunId;
            SoufunName = soufunName;
            TrueName = trueName;
            GroupId = groupId;
            GroupName = groupName;
            CompanyId = companyId;
            CompanyName = companyName;
            OrderType = orderType;
            StatusId = statusId;
            StatusName = statusName;
            UserRank = userRank;
            EnterSystemTime = grabOrderDate;
            LastFollowUpTime = createTime;
            LastFollowUpSoufunId = opId;
            LastFollowUpTrueName = opUerName;
        }
    }
}
