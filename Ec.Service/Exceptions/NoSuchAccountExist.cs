namespace Ec.Service.Exceptions;

public class NoSuchAccountExist : Exception
{
    public NoSuchAccountExist() : base("no such account exists")
    {
        
    }
}
