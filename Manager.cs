using Stride.Engine;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace StrideUtils
{
    public abstract class Manager<T>(int sleepMs)
    {
        protected readonly ConcurrentQueue<T> queue = [];
        bool running;
        readonly int sleepMs = sleepMs;
        public void Register(T entry)
        {
            if(entry == null)
                throw new ArgumentNullException(nameof(entry));
            queue.Enqueue(entry);
            if (!running)
            {
                running = true;
                Run();
            }
        }
        private async void Run()
        {
            await Task.Run(async () =>
            {
                for (; ; )
                {
                    ManageEntries();
                    await Task.Delay(sleepMs);
                }
            });
        }
        protected virtual void ManageEntries()
        {
            int count = queue.Count;
            int c = 0;
            while(c < count && queue.TryDequeue(out T entry))
            {
                c++;
                if (entry == null)
                    continue;
                ManageEntry(entry);
                queue.Enqueue(entry);
            }
        }
        protected abstract void ManageEntry(T entry);
    }
}
