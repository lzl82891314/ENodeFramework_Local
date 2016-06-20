using System.Threading.Tasks;

using ECommon.Components;
using ECommon.IO;
using ECommon.Dapper;
using ENode.Infrastructure;

using Domain.Grab;
using Infrastructure;

namespace Denormalizers.Dapper.Grab
{
    [Component]
    public class GrabOrderDenormalizer : AbstractDenormalizer, IMessageHandler<QiangLockOrderCreateEvent>, IMessageHandler<WaitQiangUpdateEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(QiangLockOrderCreateEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(
                    new
                    {
                        OrderId = evnt.OrderId,
                        GjSoufunId = evnt.GjSoufunId,
                        GjSoufunName = evnt.GjSoufunName,
                        CreateTime = evnt.CreateTime
                    }, Constants.QiangLockOrderTable);
            });

        }

        public Task<AsyncTaskResult> HandleAsync(WaitQiangUpdateEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(
                    new
                    {
                        GrabOrderDate = evnt.GrabOrderDate,
                        IsDel = evnt.IsDel
                    },
                    new
                    {
                        OrderId = evnt.OrderId
                    }, Constants.WaitQiangTempTable);
            });
        }
    }
}
