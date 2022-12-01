using System.Linq.Expressions;

namespace B1_Web.Data;

public interface IRepositiryBase<T>
{
    public T FindByFileName(Expression<Func<T, bool>> expression);
    public T FindById(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void CreateAll(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
}