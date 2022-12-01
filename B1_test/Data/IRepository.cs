using B1_test.Data.Entities;

namespace B1_test.Data;

public interface IRepository<T> : IDisposable 
    where T : class
{
    void AddString(T item);
    void Save();
}