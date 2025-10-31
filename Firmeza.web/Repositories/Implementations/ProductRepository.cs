using Firmeza.web.Data;
using Firmeza.web.Data.Entities;
using Firmeza.web.Repositories.Interfaces;

namespace Firmeza.web.Repositories.Implementations;

public class ProductRepository: GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext): base(dbContext)
    {
        
    }
}