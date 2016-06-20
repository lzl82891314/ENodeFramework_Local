using System;
using ECommon.Components;

namespace Domain.Grab
{
    public class GrabOrderLockService
    {
        public static void OrderGrabValidate(string OrderId)
        {
            var grabModel = EBS.Interface.Data.DBOper.QiangLockOrder.Get("OrderId=@orderid", "CreateTime desc", new object[] { OrderId }, false);
            if (grabModel == null || string.IsNullOrEmpty(grabModel.OrderID))
            {
                return;
            }
            else
            {
                throw new ArgumentException(string.Format("此订单：[{0}]已被抢", OrderId));
            }
        }
    }
}
