using web_api_base.Models.dbebay;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    // Add custom methods for OrderDetail here if needed
}

public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(EbayContext context) : base(context)
    {
    }
}