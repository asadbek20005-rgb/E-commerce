using Ec.Data.Entities;

namespace Ec.Data.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByPhoneNumber(string phoneNumber);


}
