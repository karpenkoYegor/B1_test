namespace B1_Web.Data;

public interface IRepositoryWrapper
{
    IBusinessAccountRepository BusinessAccount { get; }
    IIncomingBalanceRepository IncomingBalance { get; }
    IOutgoingBalanceRepository OutgoingBalance { get; }
    ITypeBusinessAccountRepository TypeBusinessAccount { get; }
    ITurnoverRepository Turnover { get; }
    IDataFileRepository DataFile { get; }
    void Save();

}