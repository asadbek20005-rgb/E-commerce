namespace Ec.Service.Exceptions;

public class SellerNotFoundException : Exception
{
    public SellerNotFoundException() : base("Seller Not Found")
    {
        
    }
}
