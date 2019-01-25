using Api.Helper.ContentWrapper.Core.BaseApiController;
using Api.Helper.ContentWrapper.Core.ResponseModel;
using Api.Helper.ContentWrapper.Core.WrapperModel;
using KNN.NULLPrinter.Core.Dto.AppUser;
using KNN.NULLPrinter.Core.Dto.Authenticate;
using KNN.NULLPrinter.Core.Dto.File;
using KNN.NULLPrinter.Core.Models;
using KNN.NULLPrinter.Service.Setting;
using KNN.NULLPrinter.Services.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Area.ApiControllers
{
    public class SettingController : BaseApiController
    {
        private ISettingService _settingService;
        private readonly IOptions<Settings> _settings;

        public SettingController(IOptions<Settings> settings, ISettingService settingService)
        {
            _settingService = settingService;
            _settings = settings;
        }

        
        [HttpGet("getEdit")]
        public async Task<ResultDto<FileDirectorySettingDto>> Get()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ApiException("User not loged in.", 401);
            }

            return await _settingService.GetEdit(); 
        }

        [HttpPut("Update")]
        public async Task<ResultDto<FileDirectorySettingDto>> Update([FromBody]FileDirectorySettingDto input)
        { 
            return await _settingService.Update(input);
        }
    }
}
