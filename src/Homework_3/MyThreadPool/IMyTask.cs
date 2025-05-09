namespace MyPool;

/// <summary>
/// Task presentation.
/// </summary>
/// <typeparam name="TResult">Type of return value.</typeparam>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Task execution status.
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    /// Task execution result.
    /// </summary>
    public TResult? Result { get; }

    /// <summary>
    /// Method that performs a task using the result of the previous task.
    /// </summary>
    /// <param name="func">Function to execute.</param>
    /// <typeparam name="TNewResult">Type of return value of new task.</typeparam>
    /// <returns>New task.</returns>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult?, TNewResult> func);
}