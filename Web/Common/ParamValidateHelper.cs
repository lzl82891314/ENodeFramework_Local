
using System;
namespace Web.Common
{
    public class ParamValidateHelper
    {
        protected string OrderId = "";
        protected long OperSoufunId = 0;
        protected string OperSoufunName = "";
        protected long OwnerSoufunId = 0;

        protected EBS.Interface.Model.Admin_UserInfo auiModel = new EBS.Interface.Model.Admin_UserInfo();
        protected EBS.Interface.Model.N_Order_Base nobModel = new EBS.Interface.Model.N_Order_Base();
        protected int DayOrderCount = 0;
        protected int DayUserCount = 0;

        public bool AllParamsValidate(out string errorMessage, out EBS.Interface.Model.Admin_UserInfo adminModel, string OrderId = "", long OperSoufunId = 0, string OperSoufunName = "", long OwnerSoufunId = 0)
        {
            adminModel = null;
            #region 1.非空验证
            if (!IsNullValidate(OrderId, OperSoufunId, OwnerSoufunId, out errorMessage))
            {
                return false;
            }
            #endregion

            //获取参数
            this.OrderId = OrderId;
            this.OperSoufunId = OperSoufunId;
            this.OwnerSoufunId = OwnerSoufunId;

            #region 2.是否是系统人员验证
            if (!IsSysOperatorValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 3.是否已经存在服务人员验证
            if (!IsExistOperatorValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 4.订单是否存在验证
            if (!IsOrderExistValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 5.订单是否被抢验证
            if (!IsGrabedValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 6.是否是管家黑名单验证
            if (!IsBlackListButlerValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 7.城市名额配置验证
            if (!GetCityConfigValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            #region 8.是否有名额验证
            if (!HasQuotaValidate(out errorMessage))
            {
                return false;
            }
            #endregion

            errorMessage = "";
            adminModel = auiModel;
            return true;
        }

        /// <summary>
        /// 非空参数验证
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        /// </summary>
        private bool IsNullValidate(string OrderId, long OperSoufunId, long OwnerSoufunId, out string errMsg)
        {
            if (string.IsNullOrEmpty(OrderId))
            {
                errMsg = "参数[OrderId]为空！";
                return false;
            }
            if (OperSoufunId <= 0)
            {
                errMsg = "参数[OperSoufunId]为空！";
                return false;
            }
            if (OwnerSoufunId <= 0)
            {
                errMsg = "参数[OwnerSoufunId]为空！";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 操作人是否是系统人员
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool IsSysOperatorValidate(out string errMsg)
        {
            auiModel = EBS.Interface.Data.DBOper.Admin_UserInfo.Get("IsDel=0 and Status=1 and SoufunID=@SoufunID", "", new object[] { OperSoufunId }, true);
            if (auiModel == null || auiModel.ID <= 0)
            {
                errMsg = "不是系统服务人员不能做该操作！";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 验证此订单是否已经存在服务人员
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool IsExistOperatorValidate(out string errMsg)
        {
            var noslModel = EBS.Interface.Data.DBOper.N_Order_ServiceLog.Get("OrderID=@OrderID and IsDel=0 and IsCurrent=1", "", new object[] { OrderId }, true);
            if (noslModel != null && noslModel.ID > 0)
            {
                errMsg = "客户被抢走啦~";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 验证该订单是否已经存在
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool IsOrderExistValidate(out string errMsg)
        {
            nobModel = EBS.Interface.Data.DBOper.N_Order_Base.Get("IsDel=0 and OrderId=@OrderId and SoufunId=@SoufunId", "CreateTime desc", new object[] { OrderId, OwnerSoufunId });
            if (nobModel == null || nobModel.ID <= 0)
            {
                errMsg = "无效的订单！";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 验证该订单是否已经被抢走
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool IsGrabedValidate(out string errMsg)
        {
            if (nobModel.GrabOrderDate > new DateTime(1900, 1, 1))
            {
                errMsg = "客户被抢走啦~";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 验证管家黑名单
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool IsBlackListButlerValidate(out string errMsg)
        {
            int countoc = EBS.Interface.Data.DBOper.Order_ConsultantBlacklist.Count(" OrderId=@OrderId and SoufunId=@SoufunId and IsDel=0 ", new object[] { OrderId, auiModel.SoufunId }, true);
            if (countoc > 0)
            {
                //该订单黑名单管家提示语
                errMsg = "该客户你不能再抢啦，已经从您那里流转过";
                InsertGuanjiaUserMap(nobModel, auiModel, 0, "该客户你不能再抢啦，已经从您那里流转过");
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 获取城市配置抢名额
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool GetCityConfigValidate(out string errMsg)
        {
            if (nobModel.CityName == "中山")
            {
                DayOrderCount = 5;
                DayUserCount = 100;
            }
            else
            {
                EBS.Interface.Model.CityInfo city = EBS.Interface.Data.DBOper.CityInfo.Get("CityName=@CityName and IsDel=0", "", new object[] { nobModel.CityName });
                if (city == null || city.CityId <= 0)
                {
                    errMsg = "暂无该城市配置信息，请联系管理人员！";
                    return false;
                }
                DayOrderCount = city.MaxQiangCountDay;
                DayUserCount = city.MaxInterUserCount;
            }
            if (DayOrderCount <= 0)
            {
                errMsg = "该城市未配置每日最大抢单数，请联系管理人员！";
                return false;
            }
            if (DayUserCount <= 0)
            {
                errMsg = "该城市未配置允许未签约最大客户数，请联系管理人员！";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 是否有名额验证
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns>结果</returns>
        private bool HasQuotaValidate(out string errMsg)
        {
            //今天抢过的单数
            long orderbaseCount = new EBS.Interface.Model.N_Order_Base().INNER_JOIN(EBS.Interface.Model.N_Order_Service.TableInfo, "ser", "SRCTAB.OrderId=ser.OrderId").Where("SRCTAB.IsDel=0 AND ser.IsDel=0 AND SRCTAB.GrabOrderDate>convert(varchar(10),getdate(),120) AND SRCTAB.UserRank<@UserRank AND ser.SoufunId=@SoufunId and SRCTAB.orderid not in (select orderid from Order_Extend where HomeClubApplyGjName=@GjName) and SRCTAB.SourceId not in (1,3,4)", new object[] { (int)EBS.BLL.EnumBLL.UserRank.E, auiModel.SoufunId, auiModel.TrueName }).Count();
            if (orderbaseCount >= DayOrderCount)
            {
                errMsg = "对不起，您今天已经超过了每日最大抢单数：" + DayOrderCount + "，明天再来吧";
                return false;
            }

            //该管家意向客户数量
            long userCount = new EBS.Interface.Model.N_Order_Base().INNER_JOIN(EBS.Interface.Model.N_Order_Service.TableInfo, "ser", "SRCTAB.OrderId=ser.OrderId").Where("SRCTAB.IsDel=0 AND ser.IsDel=0 AND SRCTAB.GrabOrderDate<='1900-01-01' AND SRCTAB.UserRank<@UserRank AND ser.SoufunId=@SoufunId", new object[] { (int)EBS.BLL.EnumBLL.UserRank.E, auiModel.SoufunId }).Count();
            if (userCount >= DayUserCount)
            {
                errMsg = "抢客失败，意向客户名单已满";
                return false;
            }
            errMsg = "";
            return true;
        }

        /// <summary>
        /// 抢客户是否成功添加日志
        /// </summary>
        /// <param name="orderbase"></param>
        /// <param name="qiangState">0失败，1成功</param>
        /// <param name="qiangRemark">错误信息</param>
        /// <returns></returns>
        protected static bool InsertGuanjiaUserMap(EBS.Interface.Model.N_Order_Base orderbase, EBS.Interface.Model.Admin_UserInfo aui, int qiangState, string qiangRemark)
        {
            int gumID = 0;
            EBS.Interface.Model.GuanjiaUserMap gum = new EBS.Interface.Model.GuanjiaUserMap();
            gum.OnwerSoufunId = orderbase.SoufunId;
            gum.OnwerSoufunName = orderbase.OrderId;
            gum.OperType = 0;//管家
            gum.OperID = aui.SoufunId;
            gum.OperName = string.IsNullOrEmpty(aui.TrueName) ? aui.SoufunName : aui.TrueName;
            gum.OperCity = orderbase.CityName;
            gum.CreateTime = DateTime.Now;
            gum.QiangState = qiangState;
            gum.QiangRemark = qiangRemark;
            return EBS.Interface.Data.DBOper.GuanjiaUserMap.Add(gum, out gumID);
        }
    }
}