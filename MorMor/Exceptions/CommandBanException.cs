namespace MorMor.Exceptions;

public class CommandBanException : Exception
{
    public CommandBanException(string messge) : base(messge)
    {

    }
}
