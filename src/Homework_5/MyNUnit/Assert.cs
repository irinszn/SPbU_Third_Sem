namespace MyNUnit;

public static class Assert
{
    public static void That(bool condition)
    {
        if (!condition)
        {
            throw new AssertFailException();
        }
    }
}