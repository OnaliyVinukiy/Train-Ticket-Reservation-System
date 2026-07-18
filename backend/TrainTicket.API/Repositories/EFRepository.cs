using Microsoft.EntityFrameworkCore;
using TrainTicket.API.Data;
using System.Linq.Expressions;

namespace TrainTicket.API.Repositories;

public class EFRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext context;
    private readonly DbSet<T> dbSet;

    public EFRepository(AppDbContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return dbSet.Find(id);
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
        context.SaveChanges();
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = dbSet.Find(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Where(predicate).ToList();
    }
}