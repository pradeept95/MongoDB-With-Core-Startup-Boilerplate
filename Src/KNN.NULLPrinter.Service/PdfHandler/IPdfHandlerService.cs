using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.File;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Service.PdfHandler
{
    public interface IPdfHandlerService
    {
        Task<ListResultDto<FileInfoDto>> GetAllPdf();
        Task<PagedResultDto<FileInfoDto>> GetAllPdfPaged(string searchText, int skip, int take);
    }
}
