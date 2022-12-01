using B1_Web.Data.Entities;

namespace B1_Web.Data;

public interface IBusinessAccountRepository : IRepositiryBase<BusinessAccount>
{
    IEnumerable<BusinessAccount> GetDataFileByFileName(string fileName);
}