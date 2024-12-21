namespace ThreadPool.Tests;

using MyPool;

public class Tests
{
    private MyThreadPool threadPool;
    private int threadsCount = Environment.ProcessorCount;

    [SetUp]
    public void Init()
    {
        threadPool = new MyThreadPool(threadsCount);
    }

    [TearDown]
    public void CleanUp()
    {
        threadPool.Shutdown();
    }

    [Test]
    public void Submit_ReturnExpectedResult()
    {
        var resultTask = threadPool.Submit(() => 2 * 2);

        var expected = 4;

        Assert.That(resultTask.Result, Is.EqualTo(expected));
    }

    [Test]
    public void ContinueWith_ReturnExpectedResult()
    {
        var task_1 = threadPool.Submit(() => 10 * 10);
        var task_2 = task_1.ContinueWith(x => x + 5);
        var task_3 = task_2.ContinueWith(x => x.ToString());

        var expected = "105";

        Assert.That(task_3.Result, Is.EqualTo(expected));

    }

    [Test]
    public void ThereAreNThread_InThreadPool()
    {
        for (var i = 0; i < threadsCount * 3; ++i)
        {
            var task = threadPool.Submit(() => 
            {
                Thread.Sleep(100);
                return 0;
            });
        }

        var expected = threadsCount;
        threadPool.Shutdown();

        Assert.That(threadPool.DoneThreadsCount, Is.EqualTo(expected));
    }

    [Test]
    public void IsCompleted_ReturnExpectedStatus()
    {
        var mre = new ManualResetEvent(false);

        var task = threadPool.Submit(() => 
        {
            mre.WaitOne();

            return 0;
        });

        Assert.That(task.IsCompleted, Is.False);

        mre.Set();
        Thread.Sleep(200);

        Assert.That(task.IsCompleted, Is.True);
    }

    [Test]
    public void AllTasksIsCompleted_AfterShutdown()
    {
        var flag = false;

        threadPool.Submit(() =>
        {
            Thread.Sleep(300);
            Volatile.Write(ref flag, true);
            return 0;
        });

        threadPool.Shutdown();

        Assert.That(flag, Is.True);
    }

    [Test]
    public void CalculateTask_WithErrorInFunction_ThrowAggregateException()
    {
        var task = threadPool.Submit(() =>
            {
                var array = new int[] {1, 2, 3};
                var temp = array[0] - 2;
                return array[temp];
            });

        Assert.Throws<AggregateException>(() => 
        {
            var result = task.Result;
        });
    }

    [Test]
    public void ContinueWith_AfterShutdown_ThrowException()
    {
        var task_1 = threadPool.Submit(() => 2 * 2);

        threadPool.Shutdown();

        Assert.Throws<InvalidOperationException>(() => task_1.ContinueWith(x => x.ToString()));
    }

    [Test]
    public void Submit_AfterShutdown_ThrowException()
    {
        threadPool.Shutdown();

        Assert.Throws<InvalidOperationException>(() => threadPool.Submit(() => 4));
    }
}