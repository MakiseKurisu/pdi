using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace pdi.Extensions
{
    public static class WaitHandleExtensions
    {
        public static TaskAwaiter<bool> GetAwaiter(this IAsyncResult result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            return result.AsyncWaitHandle.GetAwaiter();
        }

        public static TaskAwaiter<bool> GetAwaiter(this WaitHandle handle)
        {
            _ = handle ?? throw new ArgumentNullException(nameof(handle));

            return handle.WaitOneAsync().GetAwaiter();
        }

        public static async Task<bool> WaitOneAsync(this WaitHandle handle, int millisecondsTimeout = Timeout.Infinite, CancellationToken? cancellationToken = null)
        {
            _ = handle ?? throw new ArgumentNullException(nameof(handle));

            var state = new TaskCompletionSource<bool>();

            using var reg = cancellationToken?.Register((state) =>
            {
                if (state is TaskCompletionSource<bool> s)
                {
                    s.TrySetCanceled(cancellationToken.Value);
                }
                else
                {
                    throw new ArgumentException($"{nameof(state)} is not a valid TaskCompletionSource<bool> object.", nameof(state));
                }
            }, state);

            RegisteredWaitHandle? wait = null;
            try
            {
                wait = ThreadPool.RegisterWaitForSingleObject(handle, (state, timeout) =>
                {
                    if (state is TaskCompletionSource<bool> s)
                    {
                        s.TrySetResult(!timeout);
                    }
                    else
                    {
                        throw new ArgumentException($"{nameof(state)} is not a valid TaskCompletionSource<bool> object.", nameof(state));
                    }
                }, state, millisecondsTimeout, true);

                return await state.Task;
            }
            finally
            {
                wait?.Unregister(handle);
            }
        }
    }
}
