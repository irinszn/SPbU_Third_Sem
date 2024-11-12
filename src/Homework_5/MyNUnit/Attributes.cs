namespace MyNUnit;

/// <summary>
/// Class that implements Test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class TestAttribute : Attribute
{
    public Type? Expected { get; }
    public string? Ignore { get; }

    public TestAttribute(Type? expected = null, string? ignore = null)
    {
        Expected = expected;
        Ignore = ignore;
    }

    public TestAttribute(string ignore)
    {
        Expected = null;
        Ignore = ignore;
    }
}

/// <summary>
/// Class that implements Before test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class BeforeAttribute : Attribute { }

/// <summary>
/// Class that implements After test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AfterAttribute : Attribute { }

/// <summary>
/// Class that implements before class of test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class BeforeClassAttribute : Attribute { }

/// <summary>
/// Class that implements after class of test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AfterClassAttribute : Attribute { }

