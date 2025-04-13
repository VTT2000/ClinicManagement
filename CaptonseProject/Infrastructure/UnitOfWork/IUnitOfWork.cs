using web_api_base.Models.dbebay;
public interface IUnitOfWork : IAsyncDisposable
{
    public IUserRepository _userRepository{get;}
    // public IUserRoleRepository _userRoleRepository{get;}
    public IRoleRepository _roleRepository{get;}

    public IProductRepository _productRepository{get;}
    public IProductImageRepository _productImageRepository{get;}
    public IOrderRepository _orderRepository{get;}
    public IOrderDetailRepository _orderDetailRepository{get;}
    public IListingRepository _listingRepository{get;}

    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{
     public IUserRepository _userRepository{get;}
    // public IUserRoleRepository _userRoleRepository{get;}
    public IRoleRepository _roleRepository{get;}
    public IProductRepository _productRepository{get;}
    public IProductImageRepository _productImageRepository{get;}
    public IOrderRepository _orderRepository{get;}
    public IListingRepository _listingRepository{get;}
    public IOrderDetailRepository _orderDetailRepository{get;}


    private readonly EbayContext _context;
    
    public UnitOfWork(EbayContext context, IUserRepository userRepository, IRoleRepository roleRepository , IProductRepository productRepository , IProductImageRepository productImageRepository, IOrderRepository orderRepository,IListingRepository listingRepository,IOrderDetailRepository orderDetailRepository)
    {
        _context = context;
        _userRepository = userRepository;
        // _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _orderRepository = orderRepository;
        _listingRepository = listingRepository;
        _orderDetailRepository = orderDetailRepository;
    }
    public async Task BeginTransaction() {
        await _context.Database.BeginTransactionAsync();
    }
      public async Task CommitTransaction() {
        await _context.Database.CommitTransactionAsync();
    }
    public async Task RollBack() {
        await _context.Database.RollbackTransactionAsync();
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}




