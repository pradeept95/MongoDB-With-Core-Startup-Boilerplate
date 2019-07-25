using Api.ResultWrapper.AspNetCore.ResponseModel;
using Application.Core.Dto.File;
using System.Threading.Tasks;

namespace Application.Service.PdfHandler
{
    public interface IPdfHandlerService
    {
        Task<ListResultDto<FileInfoDto>> GetAllPdf();
        Task<PagedResultDto<FileInfoDto>> GetAllPdfPaged(string searchText, int skip, int take);
    }
}
