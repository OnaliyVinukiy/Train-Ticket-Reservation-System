using System.Linq.Expressions;

namespace TrainTicket.API.Repositories;

public class MemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> items = new();
    private int nextId = 1;

    public IEnumerable<T> GetAll()
    {
        return items;
    }

    public T? GetById(int id)
    {
        return items.FirstOrDefault(x =>
        {
            var property = x.GetType().GetProperty("Id");
            if (property == null)
                return false;
            var value = property.GetValue(x);
            return value != null && (int)value == id;
        });
    }

    public void Add(T entity)
    {
        var property = entity.GetType().GetProperty("Id");
        if (property != null)
            property.SetValue(entity, nextId++);
        items.Add(entity);
    }

    public void Update(T entity)
    {
        var property = entity.GetType().GetProperty("Id");
        if (property == null)
            return;
        var id = (int)property.GetValue(entity)!;
        var existing = GetById(id);
        if (existing != null)
        {
            items.Remove(existing);
            items.Add(entity);
        }
    }

    public void Delete(int id)
    {
        var item = GetById(id);
        if (item != null)
            items.Remove(item);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return items.AsQueryable().Where(predicate).ToList();
    }
}