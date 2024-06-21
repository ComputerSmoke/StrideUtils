using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrideUtils
{
    public class LoaderTimeoutException(string msg) : Exception(msg);
    public static class SpinLoader
    {
        //Spin wait for precondition to be satisfied, then call function
        public static async Task<T> Run<T>(Func<bool> Predicate, Func<T> Result, int timeout=60_000)
        {
            for(int i = 0; i <= 1+timeout/100; i++)
            {
                if (Predicate())
                    return Result();
                await Task.Delay(Math.Min(timeout, 100));
            }
            throw new LoaderTimeoutException("Loader timed out, precondition was not satisfied.");
        }
    }
}
