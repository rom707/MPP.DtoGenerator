using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Security.Permissions;

namespace DtoGenerator.ThreadPool
{
    public class CustomThreadPool : IThreadPool
    {
        private readonly int _concurrencyLevel;
        private readonly bool _flowExecutionContext;
        private readonly Queue<WorkItem> _queue = new Queue<WorkItem>();
        private Thread[] _threads;
        private int _threadsWaiting;
        private bool _shutdown;

        public CustomThreadPool() :
            this(Environment.ProcessorCount, true)
        { }
        public CustomThreadPool(int concurrencyLevel) :
            this(concurrencyLevel, true)
        { }
        public CustomThreadPool(bool flowExecutionContext) :
            this(Environment.ProcessorCount, flowExecutionContext)
        { }

        public CustomThreadPool(int concurrencyLevel, bool flowExecutionContext)
        {
            if (concurrencyLevel <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(concurrencyLevel), "Value should be more than 0");
            }

            _concurrencyLevel = concurrencyLevel;
            _flowExecutionContext = flowExecutionContext;

            if (!flowExecutionContext)
            {
                new SecurityPermission(SecurityPermissionFlag.Infrastructure).Demand();
            }
        }

        struct WorkItem
        {
            internal WaitCallback _work;
            internal object _obj;
            internal ExecutionContext _executionContext;

            internal WorkItem(WaitCallback work, object obj)
            {
                _work = work;
                _obj = obj;
                _executionContext = null;
            }

            internal void Invoke()
            {
                if (_executionContext == null)
                {
                    _work(_obj);
                }
                else
                {
                    ExecutionContext.Run(_executionContext, ContextInvoke, null);
                }
            }

            private void ContextInvoke(object obj)
            {
                _work(_obj);
            }
        }

        public void QueueUserWorkItem(WaitCallback work)
        {
            QueueUserWorkItem(work, null);
        }

        public void QueueUserWorkItem(WaitCallback work, object obj)
        {
            WorkItem wi = new WorkItem(work, obj);

            if (_flowExecutionContext)
            {
                wi._executionContext = ExecutionContext.Capture();
            }

            EnsureStarted();

            lock (_queue)
            {
                _queue.Enqueue(wi);
                if (_threadsWaiting > 0)
                {
                    Monitor.Pulse(_queue);
                }
            }
        }

        private void EnsureStarted()
        {
            if (_threads == null)
            {
                lock (_queue)
                {
                    if (_threads == null)
                    {
                        _threads = new Thread[_concurrencyLevel];

                        for (int i = 0; i < _threads.Length; i++)
                        {
                            _threads[i] = new Thread(DispatchLoop);
                            _threads[i].Start();
                        }
                    }
                }
            }
        }

        private void DispatchLoop()
        {
            while (true)
            {
                WorkItem wi = default(WorkItem);
                lock (_queue)
                {
                    if (_shutdown)
                    {
                        return;
                    }

                    while (_queue.Count == 0)
                    {
                        _threadsWaiting++;
                        try
                        {
                            Monitor.Wait(_queue);
                        }
                        finally
                        {
                            _threadsWaiting--;
                        }

                        if (_shutdown)
                        {
                            return;
                        }
                    }

                    wi = _queue.Dequeue();
                }

                wi.Invoke();
            }
        }

        public void Dispose()
        {
            _shutdown = true;
            lock (_queue)
            {
                Monitor.PulseAll(_queue);
            }

            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i].Join();
            }
        }
    }
}
