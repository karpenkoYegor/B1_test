using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class TurnoverRepository : RepositoryBase<Turnover>, ITurnoverRepository
{
    public TurnoverRepository(AppDbContext context) : base(context)
    {
    }
}