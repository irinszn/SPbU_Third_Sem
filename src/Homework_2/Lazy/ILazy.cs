namespace Lazy;

/// <summary>
/// Implementation of lazy initialization.
/// </summary>
/// <typeparam name="T">Type of initialization object.</typeparam>
public interface ILazy<T>
{
    /// <summary>
    /// Gets the initialized value.
    /// </summary>
    /// <returns>Value.</returns>
    public T? Get();
}