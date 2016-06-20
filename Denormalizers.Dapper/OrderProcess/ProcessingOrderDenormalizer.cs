using System;
using System.Threading.Tasks;

using ECommon.Components;
using ECommon.IO;
using ECommon.Dapper;
using ENode.Infrastructure;

using Domain.OrderProcess;
using Infrastructure;

namespace Denormalizers.Dapper.OrderProcess
{
    [Component]
    public class ProcessingOrderDenormalizer : AbstractDenormalizer, IMessageHandler<NOrderBaseUpdateEvent>, IMessageHandler<RealSiteInfoUpdateEvent>, IMessageHandler<OrderFollowUpCreateEvent>, IMessageHandler<NOrderServiceUpdateEvent>, IMessageHandler<NOrderServiceCreateEvent>, IMessageHandler<NOrderServiceLogUpdateEvent>, IMessageHandler<NOrderServiceLogCreateEvent>, IMessageHandler<AttentionOrderUpdateEvent>, IMessageHandler<NCustomerListUpdateEvent>, IMessageHandler<OrderFollowUpCreateSuccessEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(NOrderBaseUpdateEvent message)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                     new
                     {
                         UserRank = message.UserRank,
                         StatusId = message.StatusId,
                         StatusName = message.StatusName,
                         OrderType = message.OrderType,
                         GrabOrderDate = message.GrabOrderDate
                     },
                     new
                     {
                         OrderId = message.OrderId,
                         IsDel = 0
                     }, Constants.NOrderBaseTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(RealSiteInfoUpdateEvent message)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                    new
                    {
                        OrderType = message.OrderType,
                        GenjinStageId = message.StatusId,
                        GenjinStageName = message.StatusName
                    },
                    new
                    {
                        OrderId = message.OrderId,
                        IsDel = 0
                    }, Constants.RealSiteInfoTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(OrderFollowUpCreateEvent message)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(
                    new
                    {
                        OrderId = message.OrderId,
                        SoufunId = message.SoufunId,
                        OrderMode = message.OrderMode,
                        FollowStageId = message.FollowStageId,
                        FollowName = message.FollowName,
                        FollowDesc = message.FollowDesc,
                        IdentityType = message.IdentityType,
                        LogType = message.LogType,
                        OpId = message.OpId,
                        OpUerName = message.OpUerName,
                        OpLog = "",
                        PageSource = message.PageSource,
                        PicS = "",
                        Postion = "",
                        PosX = message.PosX,
                        PosY = message.PosY,
                        CreateTime = message.CreateTime,
                        IsDel = 0,
                        IsRating = 0,
                        GoodCount = 0,
                        CommentCount = 0,
                        BackIdentityType = 0,
                        IsFreshFlowers = 0,
                        FlowersCount = 0,
                        EggCount = 0
                    }, Constants.OrderFollowUpTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NOrderServiceUpdateEvent message)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                    new
                    {
                        SoufunId = message.SoufunId,
                        SoufunName = message.SoufunName,
                        CompanyId = message.CompanyId,
                        CompanyName = message.CompanyName,
                        GroupId = message.GroupId,
                        GroupName = message.GroupName,
                        IsServiceUser = message.IsServiceUser,
                        Phone = message.Phone,
                        TrueName = message.TrueName,
                        ServiceId = message.ServiceId,
                        ServiceName = message.ServiceName,
                        UpdateTime = message.UpdateTime
                    },
                    new
                    {
                        OrderId = message.OrderId,
                        FunctionId = message.FunctionId,
                        IsDel = 0
                    }, Constants.NOrderServiceTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NOrderServiceCreateEvent message)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(
                    new
                    {
                        OrderId = message.OrderId,
                        FunctionId = message.FunctionId,
                        SoufunId = message.SoufunId,
                        SoufunName = message.SoufunName,
                        CompanyId = message.CompanyId,
                        CompanyName = message.CompanyName,
                        GroupId = message.GroupId,
                        GroupName = message.GroupName,
                        IsServiceUser = message.IsServiceUser,
                        Phone = message.Phone,
                        TrueName = message.TrueName,
                        ServiceId = message.ServiceId,
                        ServiceName = message.ServiceName,
                        CreateTime = message.UpdateTime,
                        UpdateTime = new DateTime(1900, 01, 01),
                        IsDel = 0
                    }, Constants.NOrderServiceTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NOrderServiceLogUpdateEvent message)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                    new
                    {
                        IsCurrent = message.IsCurrent,
                        StopTime = message.NewStopTime
                    },
                    new
                    {
                        OrderId = message.OrderId,
                        IsCurrent = 0,
                        IsDel = 0,
                        ServiceId = message.ServiceId,
                        StopTime = message.OldStopTime
                    }, Constants.NOrderServiceLogTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NOrderServiceLogCreateEvent message)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(
                    new
                    {
                        OrderId = message.OrderId,
                        ServiceId = message.ServiceId,
                        SoufunId = message.SoufunId,
                        SoufunName = message.SoufunName,
                        Cause = message.Cause,
                        Remark = message.Remark,
                        IsDel = 0,
                        IsCurrent = 1,
                        CreateTime = message.CreateTime,
                        StopTime = new DateTime(1900, 01, 01)
                    }, Constants.NOrderServiceLogTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AttentionOrderUpdateEvent message)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                    new
                    {
                        IsDel = 1
                    },
                    new
                    {
                        OrderId = message.OrderId,
                        SoufunId = message.SoufunId,
                        IsDel = 0
                    }, Constants.AttentionOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NCustomerListUpdateEvent message)
        {
            if (message.FunctionId == 1)//家装顾问 jzgw
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            JZGWSoufunId = message.SoufunId,
                            JZGWSoufunName = message.SoufunName,
                            JZGWTrueName = message.TrueName,
                            JZGWGroupId = message.GroupId,
                            JZGWGroupName = message.GroupName,
                            JZGWCompanyId = message.CompanyId,
                            JZGWCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
            else if (message.FunctionId == 2)//设计师 designer
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            DesignerSoufunId = message.SoufunId,
                            DesignerSoufunName = message.SoufunName,
                            DesigneTrueName = message.TrueName,
                            DesigneGroupId = message.GroupId,
                            DesigneGroupName = message.GroupName,
                            DesigneCompanyId = message.CompanyId,
                            DesigneCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
            else if (message.FunctionId == 3)//项目经理 manager
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            ManagerSoufunId = message.SoufunId,
                            ManagerSoufunName = message.SoufunName,
                            ManagerTrueName = message.TrueName,
                            ManagerGroupId = message.GroupId,
                            ManagerGroupName = message.GroupName,
                            ManagerCompanyId = message.CompanyId,
                            ManagerCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
            else if (message.FunctionId == 4)//Supervisor
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            SupervisorSoufunId = message.SoufunId,
                            SupervisorSoufunName = message.SoufunName,
                            SupervisorTrueName = message.TrueName,
                            SupervisorGroupId = message.GroupId,
                            SupervisorGroupName = message.GroupName,
                            SupervisorCompanyId = message.CompanyId,
                            SupervisorCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
            else if (message.FunctionId == 6)//marketing
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            MarketingSoufunId = message.SoufunId,
                            MarketingSoufunName = message.SoufunName,
                            MarketingTrueName = message.TrueName,
                            MarketingGroupId = message.GroupId,
                            MarketingGroupName = message.GroupName,
                            MarketingCompanyId = message.CompanyId,
                            MarketingCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
            else
            {
                return TryUpdateRecordAsync(connection =>
                {
                    return connection.UpdateAsync(
                        new
                        {
                            StatusId = message.StatusId,
                            StatusName = message.StatusName,
                            UserRank = message.UserRank,
                            OrderType = message.OrderType,
                            EnterSystemTime = message.EnterSystemTime,
                            LastFollowUpTime = message.LastFollowUpTime,
                            LastFollowUpSoufunId = message.LastFollowUpSoufunId,
                            LastFollowUpTrueName = message.LastFollowUpTrueName,
                            JZGWSoufunId = message.SoufunId,
                            JZGWSoufunName = message.SoufunName,
                            JZGWTrueName = message.TrueName,
                            JZGWGroupId = message.GroupId,
                            JZGWGroupName = message.GroupName,
                            JZGWCompanyId = message.CompanyId,
                            JZGWCompanyName = message.CompanyName
                        },
                        new
                        {
                            OrderId = message.OrderId,
                            IsDel = 0
                        }, Constants.NCustomerListTable);
                });
            }
        }

        public Task<AsyncTaskResult> HandleAsync(OrderFollowUpCreateSuccessEvent message)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(
                    new
                    {
                        OrderId = message.OrderId,
                        SoufunId = message.SoufunId,
                        OrderMode = message.OrderMode,
                        FollowStageId = message.FollowStageId,
                        FollowName = message.FollowName,
                        FollowDesc = message.FollowDesc,
                        IdentityType = message.IdentityType,
                        LogType = message.LogType,
                        OpId = message.OpId,
                        OpUerName = message.OpUerName,
                        OpLog = "",
                        PageSource = message.PageSource,
                        PicS = "",
                        Postion = "",
                        PosX = message.PosX,
                        PosY = message.PosY,
                        CreateTime = message.CreateTime,
                        IsDel = 0,
                        IsRating = 0,
                        GoodCount = 0,
                        CommentCount = 0,
                        BackIdentityType = 0,
                        IsFreshFlowers = 0,
                        FlowersCount = 0,
                        EggCount = 0
                    }, Constants.OrderFollowUpTable);
            });
        }
    }
}
