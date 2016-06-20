using System;

using ENode.Domain;

namespace Domain.OrderProcess
{
    public class ProcessingOrder : AggregateRoot<string>
    {
        private string _orderId;

        public ProcessingOrder(string id, string orderId, long operSoufunId, long ownerSoufunId)
            : base(id)
        {
            var nobModel = EBS.Interface.Data.DBOper.N_Order_Base.Get("IsDel=0 and OrderId=@orderId and SoufunId=@ownerSoufunId", "CreateTime desc", new object[] { orderId, ownerSoufunId }, false);
            var auiModel = EBS.Interface.Data.DBOper.Admin_UserInfo.Get("IsDel=0 and Status=1 and SoufunId=@operSoufunId", "CreateTime desc", new object[] { operSoufunId }, false);
            var asaModel = EBS.Interface.Data.DBOper.Admin_SubAuth.Get("IsDel=0 and CONSTCODE=@code", "CreateTime desc", new object[] { "ADD_CUSTOMER" }, false);
            var afiModel = EBS.Interface.Data.DBOper.Admin_FunctionInfo.Get("IsDel=0 and Id=@functionId", "CreateTime desc", new object[] { auiModel.FunctionId }, false);
            var nsiModel = EBS.Interface.Data.DBOper.N_Service_Info.Get("IsDel=0 and Id=@serviceId", "CreateTime desc", new object[] { asaModel.OrderService }, false);

            var noslModel = EBS.Interface.Data.DBOper.N_Order_ServiceLog.Get("OrderId=@OrderId AND IsDel=0 AND ServiceId=@ServiceId AND StopTime='1900-01-01' and IsCurrent=1", "", new object[] { orderId, asaModel.OrderService }, true);

            int statusId = 10010;
            string statusName = "前期沟通";
            int userRank = (int)EBS.BLL.EnumBLL.UserRank.未评级;
            int orderType = auiModel.UserType;
            DateTime nowTime = DateTime.Now;

            int serviceId = asaModel.OrderService;
            string causeText = "订单服务";
            string remarkText = asaModel.AuthName;
            long noslSoufunId = noslModel.SoufunId;

            string authDefineText = string.Format("名称：{0}，ID：{1}", asaModel.AuthName, asaModel.ID);
            string followUpDesc = string.Format("【{0}】SoufunId：{1}>成为订单服务人员，人员职能：{2}，职能ID：{3}，通过服务：{4}，权限：{5}", auiModel.TrueName, auiModel.SoufunId, afiModel.FunctionName, afiModel.ID, nsiModel.ServiceName, authDefineText);

            ENode.Eventing.IDomainEvent norderserviceEvent = null;
            var nosModel = EBS.Interface.Data.DBOper.N_Order_Service.Get("IsDel=0 and OrderId=@orderId and FunctionId=@functionId", "Id desc", new object[] { orderId, afiModel.ID }, false);

            if (nosModel != null && nosModel.ID > 0)
            {
                norderserviceEvent = new NOrderServiceUpdateEvent(this, orderId, auiModel.CompanyId, auiModel.CompanyName, auiModel.GroupId, auiModel.GroupName, afiModel.IsOrderService, auiModel.Mobile, auiModel.SoufunId, auiModel.SoufunName, auiModel.TrueName, afiModel.ID, serviceId, nsiModel.ServiceName, nowTime);
            }
            else
            {
                norderserviceEvent = new NOrderServiceCreateEvent(this, orderId, auiModel.CompanyId, auiModel.CompanyName, auiModel.GroupId, auiModel.GroupName, afiModel.IsOrderService, auiModel.Mobile, auiModel.SoufunId, auiModel.SoufunName, auiModel.TrueName, afiModel.ID, serviceId, nsiModel.ServiceName, nowTime);
            }

            ApplyEvents(new NOrderBaseUpdateEvent(this, orderId, statusId, statusName, userRank, orderType, nowTime),
                        new RealSiteInfoUpdateEvent(this, orderId, 10010, statusName, orderType),
                        new NOrderServiceLogUpdateEvent(this, orderId, serviceId, nowTime),
                        new NOrderServiceLogCreateEvent(this, orderId, serviceId, auiModel.SoufunId, auiModel.SoufunName, causeText, remarkText, nowTime),
                        new AttentionOrderUpdateEvent(this, orderId, noslSoufunId),
                        new OrderFollowUpCreateEvent(this, orderId, ownerSoufunId, statusId, statusName, followUpDesc, nowTime),
                        norderserviceEvent,
                        new NCustomerListUpdateEvent(this, orderId, afiModel.ID, auiModel.SoufunId, auiModel.SoufunName, auiModel.TrueName, auiModel.GroupId, auiModel.GroupName, auiModel.CompanyId, auiModel.CompanyName, orderType, statusId, statusName, userRank, nowTime, DateTime.Now, auiModel.SoufunId, string.IsNullOrEmpty(auiModel.TrueName) ? auiModel.SoufunName : auiModel.TrueName),
                        new OrderFollowUpCreateSuccessEvent(this, orderId, ownerSoufunId, 0, "前期沟通", "管家抢客户，前期沟通", DateTime.Now, 1, nobModel.SignContractDate > new DateTime(1900, 01, 01) ? 1 : 0, auiModel.SoufunId, string.IsNullOrEmpty(auiModel.TrueName) ? auiModel.SoufunName : auiModel.TrueName, auiModel.FunctionId)
                        );
        }

        private void Handle(NOrderBaseUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(RealSiteInfoUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(NOrderServiceLogUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(NOrderServiceLogCreateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(AttentionOrderUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(OrderFollowUpCreateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(NOrderServiceUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(NOrderServiceCreateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(NCustomerListUpdateEvent evnt)
        {
            _orderId = evnt.OrderId;
        }

        private void Handle(OrderFollowUpCreateSuccessEvent evnt)
        {
            _orderId = evnt.OrderId;
        }
    }
}
