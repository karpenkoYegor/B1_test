using B1_Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace B1_Web.Data;

public class BusinessAccountRepository : RepositoryBase<BusinessAccount>, IBusinessAccountRepository
{
    public BusinessAccountRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<BusinessAccount> GetDataFileByFileName(string fileName)
    {
        var data = Context.BusinessAccounts
            .Include(b => b.FileName)
            .Include(b => b.IncomingBalance)
            .Include(b=>b.Turnover)
            .Include(b=>b.OutgoingBalance)
            .Include(b => b.TypeBusinessAccount)
            .Where(f => f.FileName.Name == fileName)
            .ToList();
        return data;
    }
}