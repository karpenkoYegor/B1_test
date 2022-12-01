using B1_test.Data.Entities;

namespace B1_test.Data;

public class B1Repository : IRepository<MyString>
{
    private readonly AppDbContext _context;
    public B1Repository()
    {
        _context = new AppDbContext();
    }

    public void AddString(MyString item)
    {
        _context.MyStrings.Add(item);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}