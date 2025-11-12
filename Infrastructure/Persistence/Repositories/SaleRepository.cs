using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;

namespace Firmeza.Infrastructure.Persistence.Repositories;

public class SaleRepository : GenericRepository<Sale>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}