namespace Ec.Service.Exceptions;

public class AddressNotFoundException : Exception
{
    public AddressNotFoundException() : base("Address Not Found")
    {
        
    }
}
