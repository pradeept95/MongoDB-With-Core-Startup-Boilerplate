
using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.AppUser;
using KNN.NULLPrinter.Core.Dto.Authenticate;
using KNN.NULLPrinter.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Services.AppUser
{
    public interface IAppUsersService
    {
        Task<KNN.NULLPrinter.Core.Models.AppUsers> Authenticate(LoginRequestDto input); 
        Task<IEnumerable<AppUsersDto>> GetAll();
        Task<PagedResultDto<AppUsersDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10);
        Task<AppUsersDto> Get(string id);
        Task<AppUsersDto> Save(AppUsersDto input, string userId);
        Task<AppUsersDto> Update(AppUsersDto input, string userId);
        Task<bool> ChangePassword(ChangePasswordDto input, string userId);
        Task<bool> Delete(string id);
    }
}
