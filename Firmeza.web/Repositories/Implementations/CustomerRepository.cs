using Firmeza.web.Data;
using Firmeza.web.Data.Entities;
using Firmeza.web.Repositories.Interfaces;

namespace Firmeza.web.Repositories.Implementations;

public class CustomerRepository: GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext):base(dbContext)
    {
        
    }
}