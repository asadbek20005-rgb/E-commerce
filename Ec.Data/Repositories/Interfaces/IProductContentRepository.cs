using Ec.Data.Entities;

namespace Ec.Data.Repositories.Interfaces;

public interface IProductContentRepository
{
    Task Add(ProductContent productContent);
    Task Update(ProductContent productContent);
    Task Delete(ProductContent productContent);

}
