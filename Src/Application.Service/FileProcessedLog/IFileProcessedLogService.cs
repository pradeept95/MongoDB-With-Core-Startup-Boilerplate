using Api.ResultWrapper.AspNetCore.ResponseModel;
using Application.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.AppUser
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
