using Api.ResultWrapper.AspNetCore.ResponseModel;
using Application.Core.Dto.AppUser;
using Application.Core.Dto.Authenticate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.AppUser
{
    public interface IAppUsersService
    {
        Task<Application.Core.Models.AppUsers> Authenticate(LoginRequestDto input); 
        Task<IEnumerable<AppUsersDto>> GetAll();
        Task<PagedResultDto<AppUsersDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10);
        Task<AppUsersDto> Get(string id);
        Task<AppUsersDto> Save(AppUsersDto input, string userId);
        Task<AppUsersDto> Update(AppUsersDto input, string userId);
        Task<bool> ChangePassword(ChangePasswordDto input, string userId);
        Task<bool> Delete(string id);
    }
}
