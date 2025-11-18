using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Infrastructure.Persistence.Repositories;

public class SaleRepository : GenericRepository<Sale>, ISaleRepository
{
    private readonly ApplicationDbContext _context;

    public SaleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public override async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Customer)          
            .Include(s => s.SaleDetails)       
            .ThenInclude(d => d.Product)     
            .ToListAsync();
    }

    public override async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}