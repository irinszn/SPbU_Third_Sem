namespace MyNUnit;

/// <summary>
/// Class with information about runned test.
/// </summary>
public class TestResult
{
    public string? TestName { get; set; }

    public bool Passed { get; set; }

    public string? Message { get; set; }

    public TimeSpan Duration { get; set; }
}