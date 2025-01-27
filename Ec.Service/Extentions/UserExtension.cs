using Ec.Common.Constants;
using Ec.Data.Entities;

namespace Ec.Service.Extentions;

public static class UserExtension
{
    public static void CheckUserRole(string role)
    {
        if (role != Constants.SellerRole && role != Constants.ClientRole)
            throw new Exception("Role must be client or seller");
    }

}
