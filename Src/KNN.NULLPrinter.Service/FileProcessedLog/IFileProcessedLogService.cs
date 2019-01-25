
using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.AppUser;
using KNN.NULLPrinter.Core.Dto.Authenticate;
using KNN.NULLPrinter.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Services.AppUser
{
    public interface IFileProcessedLogService
    { 
        Task<IEnumerable<FileProcessedLogDto>> GetAll();
        Task<PagedResultDto<FileProcessedLogDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10);
        Task<FileProcessedLogDto> Get(string id);
        Task<FileProcessedLogDto> Save(FileProcessedLogDto input);
        Task<FileProcessedLogDto> Update(FileProcessedLogDto input); 
        Task<bool> Delete(string id);
    }
}
