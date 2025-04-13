using web_api_base.Models.dbebay;

public interface IOrderRepository : IRepository<Order>
{
    // Add custom methods for Order here if needed
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(EbayContext context) : base(context)
    {
    }
}