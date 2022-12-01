using B1_Web.Data.Entities;

namespace B1_Web.Data;

public class DataFileRepository : RepositoryBase<DataFile>, IDataFileRepository
{
    public DataFileRepository(AppDbContext context) : base(context)
    {
    }
}