using System.Collections;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel;

public interface IUserService
{
    public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsStaffAsync();
    public Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsPatientAsync();
}

public class UserService : IUserService
{
    public IUnitOfWork _uow;
    public UserService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsStaffAsync()
    {
        var result = new HTTPResponseClient<IEnumerable<User>>();
        try
        {
            var data = await _uow._userRepository.WhereAsync(p => !p.Role.Equals(RoleUser.ADMIN) && !p.Role.Equals(RoleUser.BENH_NHAN));
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }

        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<HTTPResponseClient<IEnumerable<User>>> GetAllUserIsPatientAsync()
    {
        var result = new HTTPResponseClient<IEnumerable<User>>();
        try
        {
            var data = await _uow._userRepository.WhereAsync(p => p.Role.Equals(RoleUser.BENH_NHAN));
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }

        result.DateTime = DateTime.Now;
        return result;
    }

}