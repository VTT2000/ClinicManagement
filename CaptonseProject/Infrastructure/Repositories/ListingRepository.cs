using web_api_base.Models.dbebay;

public interface IListingRepository : IRepository<Listing>
{
    // Add custom methods for Listing here if needed
}

public class ListingRepository : Repository<Listing>, IListingRepository
{
    public ListingRepository(EbayContext context) : base(context)
    {
        
    }
}