using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.dbebay;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> SingleOrDefaultAsync(Expression<Func<Order, bool>> predicate);
    Task<IEnumerable<Order>> WhereAsync(Expression<Func<Order, bool>> predicate);
    Task AddAsync(Order entity);
    Task UpdateAsync(Order entity);
    Task DeleteAsync(int id);
}

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _repository;

    public OrderService(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Order?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Order?> SingleOrDefaultAsync(Expression<Func<Order, bool>> predicate) => await _repository.SingleOrDefaultAsync(predicate);

    public async Task<IEnumerable<Order>> WhereAsync(Expression<Func<Order, bool>> predicate) => await _repository.WhereAsync(predicate);

    public async Task AddAsync(Order entity) => await _repository.AddAsync(entity);

    public async Task UpdateAsync(Order entity) => _repository.Update(entity);

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}