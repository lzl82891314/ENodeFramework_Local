using System;
using System.Threading.Tasks;

using ECommon.IO;
using ECommon.Components;
using ECommon.Dapper;

using Infrastructure;

namespace Denormalizers.Dapper
{
    [Component]
    public class ErrMsgDenormalizer : AbstractDenormalizer
    {
        public Task<AsyncTaskResult> LoggingErrMsgToDb(string errJsonMsg = "", string errMsg = "", int errStatus = -1)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    ErrorJsonStr = errJsonMsg,
                    ErrorMessage = errMsg,
                    ErrorStatus = errStatus,
                    IsReSend = 0,
                    CreatedOn = DateTime.Now
                }, Constants.ErrorMessageTable);
            });
        }
    }
}
