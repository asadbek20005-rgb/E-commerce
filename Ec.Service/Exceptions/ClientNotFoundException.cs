namespace Ec.Service.Exceptions;

public class ClientNotFoundException : Exception
{
    public ClientNotFoundException() : base("Client Not Found")
    {
        
    }
}
