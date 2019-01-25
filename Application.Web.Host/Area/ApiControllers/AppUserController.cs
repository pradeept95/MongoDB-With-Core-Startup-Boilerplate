using Api.Helper.ContentWrapper.Core.BaseApiController;
using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.AppUser;
using KNN.NULLPrinter.Services.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Area.ApiControllers
{
    public class AppUserController : BaseApiController
    {
        private readonly IAppUsersService _appUsersService;

        public AppUserController(IAppUsersService appUsersService)
        {
            _appUsersService = appUsersService;
        }
         
        [HttpGet("GetAll")] 
        public async Task<ListResultDto<AppUsersDto>> GetAll()
        {
            var cu = AppSession.UserId;


            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var data =  await _appUsersService.GetAll(); 
            return new ListResultDto<AppUsersDto>(data.ToList());
        }

        [HttpGet("GetAllPaged")]
        public async Task<PagedResultDto<AppUsersDto>> GetAll(string searchText, int skip = 0, int maxResultCount = 10)
        { 
           return await _appUsersService.GetAllPaged(searchText, skip, maxResultCount); 
        }
         
        [HttpGet("Get/{id}")]
        public async Task<ResultDto<AppUsersDto>> Get(string id)
        {
            var result = await _appUsersService.Get(id) ?? new AppUsersDto();
            return new ResultDto<AppUsersDto>(result);
        }
          
        [HttpPost("Save")]
        public async Task<ResultDto<AppUsersDto>> Save([FromBody] AppUsersDto input)
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            var result = await _appUsersService.Save(input, userId);
            return new ResultDto<AppUsersDto>(result);
        }

        [HttpPut("Update")]
        public async Task<ResultDto<AppUsersDto>> Update([FromBody] AppUsersDto input)
        { 
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _appUsersService.Update(input, userId);
            return new ResultDto<AppUsersDto>(result);
        }

        [HttpPut("ChangePassword")]
        public async Task<ResultDto<bool>> ChangePassword([FromBody] ChangePasswordDto input)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _appUsersService.ChangePassword(input, userId);
            return new ResultDto<bool>(result);
        }

        [HttpPut("ChangeMyPassword")]
        public async Task<ResultDto<bool>> ChangeMyPassword([FromBody] ChangeMyPasswordDto input)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _appUsersService.ChangePassword(new ChangePasswordDto
            {
                Id = userId,
                Password = input.Password,
                ConfirmPassword = input.ConfirmPassword
            }, userId);
            return new ResultDto<bool>(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResultDto<bool>> Delete(string id)
        {
            await _appUsersService.Delete(id);
            return new ResultDto<bool>(true);
        }
    }
}
