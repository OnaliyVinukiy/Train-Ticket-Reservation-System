using System.Linq.Expressions;

namespace TrainTicket.API.Repositories;

public interface IRepository<T>
    where T : class
{

    IEnumerable<T> GetAll();

    T? GetById(int id);

    void Add(T entity);

    void Update(T entity);

    void Delete(int id);


    IEnumerable<T> Find(
        Expression<Func<T, bool>> predicate
    );

}