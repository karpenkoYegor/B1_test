namespace B1_Web.Data;

public class RepositoryWrapper : IRepositoryWrapper
{
    private AppDbContext _context;
    private IBusinessAccountRepository _businessAccount;
    private IIncomingBalanceRepository _incomingBalance;
    private IOutgoingBalanceRepository _outgoingBalance;
    private ITypeBusinessAccountRepository _typeBusinessAccount;
    private ITurnoverRepository _turnover;
    private IDataFileRepository _dataFile;

    public IBusinessAccountRepository BusinessAccount
    {
        get
        {
            if (_businessAccount == null)
                _businessAccount = new BusinessAccountRepository(_context);
            return _businessAccount;
        }
    }

    public IIncomingBalanceRepository IncomingBalance
    {
        get
        {
            if (_incomingBalance == null)
                _incomingBalance = new IncomingBalanceRepository(_context);
            return _incomingBalance;
        }
    }

    public IOutgoingBalanceRepository OutgoingBalance {
        get
        {
            if (_outgoingBalance == null)
                _outgoingBalance = new OutgoingBalanceRepository(_context);
            return _outgoingBalance;
        }
    }
    public ITurnoverRepository Turnover {
        get
        {
            if (_turnover == null)
                _turnover = new TurnoverRepository(_context);
            return _turnover;
        }
    }

    public IDataFileRepository DataFile {
        get
        {
            if (_dataFile == null)
                _dataFile = new DataFileRepository(_context);
            return _dataFile;
        }
    }

    public ITypeBusinessAccountRepository TypeBusinessAccount
    {
        get
        {
            if (_typeBusinessAccount == null)
                _typeBusinessAccount = new TypeBusinessAccountRepository(_context);
            return _typeBusinessAccount;
        }
    }
    public RepositoryWrapper(AppDbContext context)
    {
        _context = context;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}