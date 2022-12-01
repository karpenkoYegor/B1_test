using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace B1_Web.Data;

public abstract class RepositoryBase<T> : IRepositiryBase<T> where T : class
{
    protected AppDbContext Context { get; set; }
    public RepositoryBase(AppDbContext context)
    {
        Context = context;
    }

    public T FindByFileName(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(expression).AsNoTracking().First();
    public T FindById(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(expression).AsNoTracking().First();
    public void Create(T entity) => Context.Set<T>().Add(entity);
    public void CreateAll(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public void Update(T entity) => Context.Set<T>().Update(entity);

    public void Delete(T entity) => Context.Set<T>().Remove(entity);
}