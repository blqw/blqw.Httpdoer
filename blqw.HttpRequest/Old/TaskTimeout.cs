using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public static class TaskTimeout
    {
        public static async Task Timeout(this Task task, int timeout, string msg = "")
        {
            var delay = Task.Delay(timeout);
            if (await Task.WhenAny(task, delay) == delay)
            {
                throw new TimeoutException(msg);
            }
        }

        public static async Task<T> Timeout<T>(this Task<T> task, int timeout, string msg = "")
        {
            await ((Task)task).Timeout(timeout, msg);
            return await task;
        }
    }
}
