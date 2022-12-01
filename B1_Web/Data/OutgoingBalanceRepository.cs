using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class OutgoingBalanceRepository : RepositoryBase<OutgoingBalance>, IOutgoingBalanceRepository
{
    public OutgoingBalanceRepository(AppDbContext context) : base(context)
    {
    }
}