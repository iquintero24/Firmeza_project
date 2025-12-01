using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;



namespace Firmeza.Infrastructure.Persistence.Repositories;

public class CustomerRepository: GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext):base(dbContext)
    {
        
    }
}