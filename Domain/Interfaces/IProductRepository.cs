using Firmeza.Domain.Base.Interfaces;
using Firmeza.Domain.Entities;


namespace Firmeza.Domain.Interfaces;

public interface IProductRepository: IGenericRepository<Product>
{
    // this agg method specific that product need 
}