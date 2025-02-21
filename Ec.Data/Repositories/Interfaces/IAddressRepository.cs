using Ec.Data.Entities;

namespace Ec.Data.Repositories.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address> GetAddressByName(Guid sellerId,string address);
    Task<Address> GetAddressBySellerId(Guid sellerId);
}
