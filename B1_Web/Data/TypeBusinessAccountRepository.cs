using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class TypeBusinessAccountRepository : RepositoryBase<TypeBusinessAccount>, ITypeBusinessAccountRepository
{
    public TypeBusinessAccountRepository(AppDbContext context) : base(context)
    {
    }
}