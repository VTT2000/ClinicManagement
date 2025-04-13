using web_api_base.Models.dbebay;

public interface IProductImageRepository : IRepository<ProductImage>
{
    // Add custom methods for ProductImage here if needed
}

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(EbayContext context) : base(context)
    {
    }
}