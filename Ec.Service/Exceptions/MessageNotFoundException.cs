namespace Ec.Service.Exceptions;

public class MessageNotFoundException : Exception
{
    public MessageNotFoundException() : base("Message Not Found")
    {
        
    }
}
