namespace TrainTicket.API.Repositories;

public class MemoryRepository<T> : IRepository<T>
{
    private readonly List<T> items = new();


    public IEnumerable<T> GetAll()
    {
        return items;
    }


    public T? GetById(int id)
    {
        return items
            .Where(x =>
                x!.GetType()
                .GetProperty("Id")?
                .GetValue(x)?
                .Equals(id) == true)
            .FirstOrDefault();
    }


    public void Add(T entity)
    {
        items.Add(entity);
    }


    public void Update(T entity)
    {
        var existing = GetById(
            (int)entity!
            .GetType()
            .GetProperty("Id")!
            .GetValue(entity)!
        );


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
        {
            items.Remove(item);
        }
    }
}