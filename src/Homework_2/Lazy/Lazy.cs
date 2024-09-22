namespace Lazy;

/// <summary>
/// Class that implements single-threaded lazy initialization.
/// </summary>
/// <typeparam name="T">Type of initialization object.</typeparam>
public class Lazy<T> : ILazy<T>
{
    private T? value;
    private Func<T>? supplier;
    private bool isValueCreated;
    private Exception? exception;

    /// <summary>
    /// Constructor of Lazy object.
    /// </summary>
    /// <param name="supplier">Not nullable function.</param>
    public Lazy(Func<T> supplier)
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
            try
            {
                value = supplier!();
                isValueCreated = true;
                supplier = default;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                throw new InvalidOperationException("Error.");
            }
        }

        return value;
    }
}