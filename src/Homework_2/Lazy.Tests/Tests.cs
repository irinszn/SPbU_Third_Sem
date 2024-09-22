namespace LazyTests;

using Lazy;

public class LazyTests
{
    private static int count;

    private static IEnumerable<TestCaseData> SimpleLazy
    {
        get
        {
            yield return new TestCaseData(new Lazy<int> (() => 10));
            yield return new TestCaseData(new ParallelLazy<int> ((() => 10)));
        }
    }

    private static IEnumerable<TestCaseData> SimpleLazyWithCount
    {
        get
        {
            yield return new TestCaseData(new Lazy<int>(() => 
            {
                count++;
                return 10;
            }));
            yield return new TestCaseData(new ParallelLazy<int>(() => 
            {
                count++;
                return 10;
            }));
        }
    }

    private static IEnumerable<TestCaseData> LazyException
    {
        get
        {
            yield return new TestCaseData(new Lazy<int>(() => 
            {
                var array = new int[] {1, 2, 3};
                var temp = array[0] - 2;
                return array[temp];
            }));
        }
    }

    [TestCaseSource(nameof(SimpleLazy))]
    public void ValueCreated_OnTheFirstAccess(ILazy<int> lazy)
    {
        var expected = 10;

        Assert.That(lazy.Get(), Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(SimpleLazyWithCount))]
    public void ValueShouldNotCreate_AfterFirstInitialization(ILazy<int> lazy)
    {
        var expected = 1;
        count = 0;

        var firstValue = lazy.Get();
        var secondValue = lazy.Get();

        Assert.That(firstValue, Is.EqualTo(secondValue));
        Assert.That(count, Is.EqualTo(expected));
    }

    [Test]
    public void NullConstructor_ThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Lazy<int>(null));
        Assert.Throws<ArgumentNullException>(() => new ParallelLazy<int>(null));
    }

    [TestCaseSource(nameof(LazyException))]
    public void ValueAfterException_ThrowException(ILazy<int> lazy)
    {
        Assert.Throws<InvalidOperationException>(() => lazy.Get());
    }

    [Test]
    public void ValueCreateOnce_InMultithreading()
    {
        count = 0;
        var expected = 1;
        var threadsCount = Environment.ProcessorCount;

        var lazy = new ParallelLazy<int>(() => 
        {
            Interlocked.Increment(ref count);
            return 100;
        });

        var threads = new Thread[threadsCount];

        for (var i = 0; i < threadsCount; ++i)
        {
            threads[i] = new Thread(() => lazy.Get());
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Assert.That(count, Is.EqualTo(expected));
    }
}