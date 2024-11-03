namespace Lazy;

/// <summary>
/// Class that implements parallel lazy initialization.
/// </summary>
/// <typeparam name="T">Type of initialization object.</typeparam>
public class ParallelLazy<T> : ILazy<T>
{
    private readonly object locker = new ();
    private T? value;
    private Func<T>? supplier;
    private volatile bool isValueCreated;
    private Exception? supplierException;

    /// <summary>
    /// Constructor of Lazy object.
    /// </summary>
    /// <param name="supplier">Not nullable function.</param>
    public ParallelLazy(Func<T>? supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);

        this.supplier = supplier;
        isValueCreated = false;
    }

    /// <summary>
    /// Gets the initialized value.
    /// </summary>
    /// <returns>Value.</returns>
    public T? Get()
    {
        if (!isValueCreated)
        {
            lock (locker)
            {
                if (!isValueCreated)
                {
                    try
                    {
                        value = supplier!();
                        isValueCreated = true;
                        supplier = default;
                    }
                    catch (Exception ex)
                    {
                        supplierException = ex;
                    }

                    if (supplierException is not null)
                    {
                        throw supplierException;
                    }
                }
            }
        }

        return value;
    }
}