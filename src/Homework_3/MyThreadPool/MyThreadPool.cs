namespace MyPool;

/// <summary>
/// Class that implements simple thread pool.
/// </summary>
public class MyThreadPool
{
    private readonly Queue<Action> _tasks;
    private readonly Worker[] _threads;

    private readonly CancellationTokenSource _tokenSource;
    private readonly AutoResetEvent _accessToTaskQueue;
    private readonly AutoResetEvent _wakeUpEvent;
    private readonly ManualResetEvent _shutdownEvent;
    
    private int _doneThreadsCount;

    /// <summary>
    /// Number of threads that done their work.
    /// </summary>
    /// <value>Number of completed threads.</value>
    public int DoneThreadsCount
    {
        get => _doneThreadsCount;
    }

    /// <summary>
    /// Initialization of MyThreadPool.
    /// </summary>
    /// <param name="threadsCount">Input number of threads.</param>
    public MyThreadPool(int threadsCount)
    {
        if (threadsCount <= 0)
        {
            throw new ArgumentException("Number of threads must be positive.");
        }

        _tasks = new();
        _threads = new Worker[threadsCount];
        _tokenSource = new();
        _accessToTaskQueue = new(true);
        _wakeUpEvent = new(false);
        _shutdownEvent = new(false);
        _doneThreadsCount = 0;

        for (var i = 0; i < threadsCount; ++i)
        {
            _threads[i] = new Worker(this);
        }
    }

    /// <summary>
    /// Method that puts task into task queue asynchronously.
    /// </summary>
    /// <param name="func">Function to evaluate.</param>
    /// <typeparam name="TResult">Result type of function.</typeparam>
    /// <returns>Object of IMyTask.</returns>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
    {
        if (_tokenSource.Token.IsCancellationRequested)
        {
            throw new InvalidOperationException("Thread pool has stopped.");
        }

        var task = new MyTask<TResult>(this, func);

        SubmitAction(task.Calculate);

        return task;
    }

    private void SubmitAction(Action task)
    {
        _accessToTaskQueue.WaitOne();

        _tasks.Enqueue(task);

        _wakeUpEvent.Set();
        _accessToTaskQueue.Set();
    }

    /// <summary>
    /// Shuts down the thread pool.
    /// New tasks are not accepted, the started ones are being completed.
    /// </summary>
    public void Shutdown()
    {
        if (_tokenSource.Token.IsCancellationRequested)
        {
            return;
        }

        _tokenSource.Cancel();
        _shutdownEvent.Set();

        while (true)
        {
            if (_threads.Length == _doneThreadsCount)
            {
                break;
            }
        }

        _accessToTaskQueue.Dispose();
        _wakeUpEvent.Dispose();
        _shutdownEvent.Dispose();
    }

    /// <summary>
    /// Class that implements performing tasks and the work of threads in the thread pool.
    /// </summary>
    private class Worker
    {
        private readonly MyThreadPool _threadPool;

        public Worker(MyThreadPool threadPool)
        {
            _threadPool = threadPool;

            var thread = new Thread(Work);

            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Method regulating the work of threads and performing tasks.
        /// </summary>
        private void Work()
        {
            WaitHandle.WaitAny([_threadPool._wakeUpEvent, _threadPool._shutdownEvent]);

            while (true)
            {
                _threadPool._accessToTaskQueue.WaitOne();

                if (_threadPool._tasks.TryDequeue(out var task))
                {
                    _threadPool._accessToTaskQueue.Set();
                    task();
                }
                else if (_threadPool._tokenSource.Token.IsCancellationRequested)
                {
                   _threadPool._accessToTaskQueue.Set();
                   break;
                }
                else
                {
                    _threadPool._accessToTaskQueue.Set();
                    WaitHandle.WaitAny([_threadPool._wakeUpEvent, _threadPool._shutdownEvent]);
                }
            }

            Interlocked.Increment(ref _threadPool._doneThreadsCount);
        }
    }

    /// <summary>
    /// Class that implements IMyTask interface.
    /// </summary>
    /// <typeparam name="TResult">Type of function's return.</typeparam>
    private class MyTask<TResult> : IMyTask<TResult>
    {
        private readonly MyThreadPool _threadPool;
        private readonly ManualResetEvent _resultSignal;
        
        private Func<TResult>? _func;
        private Exception? _funcException;
        private TResult? _result;
        private List<Action> _continuations;

        private volatile bool _isCompleted;

        public bool IsCompleted => _isCompleted;

        public TResult? Result
        {
            get
            {
                _resultSignal.WaitOne();

                if (_funcException is not null)
                {
                    throw new AggregateException(_funcException);
                }

                return _result;
            }
        }

        public MyTask(MyThreadPool threadPool, Func<TResult> func)
        {
            _threadPool = threadPool;
            _func = func;

            _isCompleted = false;

            _resultSignal = new ManualResetEvent(false);
            _continuations = new List<Action>();
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult?, TNewResult> func)
        {
            if (_threadPool._tokenSource.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException("Thread pool has stopped.");
            }

            if (_isCompleted)
            {
                return _threadPool.Submit(() => func(Result));
            }

            var continuation = new MyTask<TNewResult>(_threadPool, () => func(Result));
            _continuations.Add(() => continuation.Calculate());

            return continuation;
        }

        /// <summary>
        /// Method that executes the function in the task asynchronously.
        /// </summary>
        internal void Calculate()
        {
            if (_func is null)
            {
                _funcException = new Exception("Function can't be null.");
                return;
            }

            try
            {
                _result = _func();
            }
            catch (Exception ex)
            {
                _funcException = new Exception(ex.Message, ex);
            }
            finally
            {
                _func = null;
                _isCompleted = true;

                _resultSignal.Set();
                SubmitContinuations();
            }
        }

        /// <summary>
        /// Sends continuations to the task queue in the thread pool.
        /// </summary>
        private void SubmitContinuations()
            => _continuations.ForEach(_threadPool.SubmitAction);
    }
}