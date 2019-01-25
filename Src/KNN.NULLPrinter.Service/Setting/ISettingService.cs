using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.File;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Service.Setting
{
    public interface ISettingService
    {
        Task<ResultDto<FileDirectorySettingDto>> GetEdit();
        Task<ResultDto<FileDirectorySettingDto>> Update(FileDirectorySettingDto input);
    }
}
