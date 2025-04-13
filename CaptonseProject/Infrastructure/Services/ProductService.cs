using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.dbebay;

public interface IProductService
{
   
    Task<HTTPResponseClient<IEnumerable<Listing>>> GetAllProductListing();
}

public class ProductService(IUnitOfWork _unitOfWork) : IProductService
{
    public async  Task<HTTPResponseClient<IEnumerable<Listing>>> GetAllProductListing()
    {
       var res  = await _unitOfWork._listingRepository.GetAllAsync();
       HTTPResponseClient<IEnumerable<Listing>> data = new HTTPResponseClient<IEnumerable<Listing>>(){
           StatusCode = 200,
           Data = res.ToList().Skip(0).Take(20),
           DateTime = DateTime.Now,
           Message = "Successfully"
       };
       return data;
    }
}