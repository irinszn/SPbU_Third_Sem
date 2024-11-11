namespace MyNUnit;

public class AssertFailException : Exception
{
    public AssertFailException()
    {
    }

    public AssertFailException(string message) : base(message)
    {
    }
}