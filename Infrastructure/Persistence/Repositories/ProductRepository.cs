using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Firmeza.Infrastructure.Persistence;


namespace Firmeza.Infrastructure.Persistence.Repositories;

public class ProductRepository: GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext): base(dbContext)
    {
        
    }
}