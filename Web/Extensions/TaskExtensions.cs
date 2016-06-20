using System;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> TimeOut<T>(this Task<T> task, long timeForMs, string msg = "")
        {
            TimeSpan timeout = new TimeSpan(timeForMs * 1000);
            var delay = Task<T>.Delay(timeout);
            if (await Task<T>.WhenAny(task, delay) == delay)
            {
                throw new TimeoutException(msg);
            }
            return await task;
        }
    }
}