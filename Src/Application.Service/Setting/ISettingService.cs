using Api.ResultWrapper.AspNetCore.ResponseModel;
using Application.Core.Dto.File;
using System.Threading.Tasks;

namespace Application.Service.Setting
{
    public interface ISettingService
    {
        Task<ResultDto<FileDirectorySettingDto>> GetEdit();
        Task<ResultDto<FileDirectorySettingDto>> Update(FileDirectorySettingDto input);
    }
}
