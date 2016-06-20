using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using ECommon.Components;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;

using Commands.Grab;
using Web.Extensions;
using Web.Common;
using Denormalizers.Dapper;

namespace Web.Controllers
{
    public class GrabOrderController : Controller
    {
        // GET: GrabOrder
        //public ActionResult Grab()
        [AsyncTimeout(2000)]
        public async Task<ActionResult> Grab()
        {
            try
            {
                #region 参数验证
                var validateResult = ParamsValidateMethod();
                if (validateResult != null)
                {
                    return validateResult;
                }
                string orderId = Request["OrderId"];
                long operSoufunId = long.Parse(Request["OperSoufunId"]);
                long ownerSoufunId = long.Parse(Request["OwnerSoufunId"]);
                string operSoufunName = Request["OperSoufunName"] ?? "";
                #endregion

                #region 数据验证

                string errMsg = string.Empty;
                EBS.Interface.Model.Admin_UserInfo auiModel = null;
                if (!new ParamValidateHelper().AllParamsValidate(out errMsg, out auiModel, orderId, operSoufunId, operSoufunName, ownerSoufunId))
                {
                    throw new Exception(errMsg);
                }
                var asaModel = EBS.Interface.Data.DBOper.Admin_SubAuth.Get("IsDel=0 and CONSTCODE=@code", "CreateTime desc", new object[] { "ADD_CUSTOMER" }, false);
                if (asaModel == null || asaModel.ID <= 0)
                {
                    throw new ArgumentException(string.Format("CONSTCODE为：[{0}]的Admin_SubAuth表信息为空！", "ADD_CUSTOMER"));
                }
                if (asaModel.OrderService <= 0)
                {
                    throw new Exception("非订单特定服务，无需操作！");
                }
                if (auiModel.FunctionId <= 0)
                {
                    throw new Exception("用户未分配职能！");
                }
                var afiModel = EBS.Interface.Data.DBOper.Admin_FunctionInfo.Get("IsDel=0 and Id=@functionId", "CreateTime desc", new object[] { auiModel.FunctionId }, false);
                if (afiModel == null || afiModel.ID <= 0)
                {
                    throw new ArgumentException(string.Format("FunctionId为：[{0}]的Admin_FunctionInfo表信息为空！", auiModel.FunctionId));
                }
                var nsiModel = EBS.Interface.Data.DBOper.N_Service_Info.Get("IsDel=0 and Id=@serviceId", "CreateTime desc", new object[] { asaModel.OrderService }, false);
                if (nsiModel == null || nsiModel.ID <= 0)
                {
                    throw new ArgumentException(string.Format("ServiceId为：[{0}]的N_Service_Info表信息为空！", asaModel.OrderService));
                }

                #endregion

                var _commandService = ObjectContainer.Resolve<ICommandService>();

                var command = new GrabOrderCommand(ObjectId.GenerateNewStringId(), orderId, operSoufunId, operSoufunName, ownerSoufunId);
                var result = await _commandService.ExecuteAsync(command, CommandReturnType.EventHandled).TimeOut(20000, "任务超时");

                if (result.Status == AsyncTaskStatus.Success)
                {
                    if (!string.IsNullOrEmpty(result.Data.ErrorMessage))
                    {
                        return Json(result.Data.ErrorMessage, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(string.Format("订单编号为：[{0}]的消息发送成功！", orderId), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (result.Status == AsyncTaskStatus.IOException)
                    {
                        var errJsonStr = CommandExecuteHelper.CreateErrJsonStr(command);
                        await new ErrMsgDenormalizer().LoggingErrMsgToDb(errJsonStr, result.ErrorMessage, (int)result.Status);
                        return Json("服务器异常", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(string.Format("发送失败，错误原因：[{0}]", result.Data.ErrorMessage), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 必填参数验证
        /// </summary>
        private ActionResult ParamsValidateMethod()
        {
            List<KeyValuePair<string, Type>> paramList = new List<KeyValuePair<string, Type>>() 
            {
                new KeyValuePair<string,Type>("OrderId",typeof(string)),
                new KeyValuePair<string,Type>("OperSoufunId",typeof(long)),
                //new KeyValuePair<string, Type>("OperSoufunName",typeof(string)),
                new KeyValuePair<string,Type>("OwnerSoufunId",typeof(long))
            };
            foreach (var item in paramList)
            {
                #region 非空验证
                if (string.IsNullOrEmpty(Request[item.Key]))
                {
                    return Json("参数[" + item.Key + "]为空！", JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region 格式验证
                var type = item.Value;
                //String类型不用使用TryParse方法匹配
                if (type.ToString() != "System.String")
                {
                    var obj = Activator.CreateInstance(type);
                    var method = type.GetMethod("TryParse", new Type[] { typeof(string), item.Value.MakeByRefType() });
                    var result = method.Invoke(null, new object[] { Request[item.Key], obj });
                    if (!result.ToString().ToUpper().Equals("TRUE"))
                    {
                        return Json("参数[" + item.Key + "]传入的格式不正确！", JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
            }
            return null;
        }
    }
}