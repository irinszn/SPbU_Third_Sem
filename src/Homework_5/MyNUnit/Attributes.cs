namespace MyNUnit;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public Type Expected { get; }
    public string Ignore { get; }

    public TestAttribute(Type expeted = null, string ignore = null)
    {
        Expected = expeted;
        Ignore = ignore;
    }

    public TestAttribute(Type expected)
    {
        Ignore = null;
        Expected = expected;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class BeforeAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class AfterAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class BeforeClassAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class AfterClassAttribute : Attribute { }

