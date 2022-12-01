using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class IncomingBalanceRepository: RepositoryBase<IncomingBalance>, IIncomingBalanceRepository
{
    public IncomingBalanceRepository(AppDbContext context) : base(context)
    {
    }
} 