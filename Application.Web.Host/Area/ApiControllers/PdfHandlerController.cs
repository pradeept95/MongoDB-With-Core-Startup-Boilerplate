using Api.Helper.ContentWrapper.Core.BaseApiController;
using Api.Helper.ContentWrapper.Core.ResponseModel;
using Application.Core.Dto.File;
using Application.Service.PdfHandler;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Area.ApiControllers
{
    public class PdfHandlerController : BaseApiController
    {
        private readonly IPdfHandlerService _pdfHandlerService;

        public PdfHandlerController(IPdfHandlerService pdfHandlerService)
        {
            _pdfHandlerService = pdfHandlerService;  
        } 

        [HttpGet("GetAll")]
        public async Task<ListResultDto<FileInfoDto>> GetAll()
        {
            //SetUserClaimInfo(); 
            var files =  await _pdfHandlerService.GetAllPdf();
            return files; 
        }

        [HttpGet("GetAllPaged")]
        public async Task<PagedResultDto<FileInfoDto>> GetAllPaged(string searchText, int skip = 0, int take = 10)
        {
            //SetUserClaimInfo(); 
            var files = await _pdfHandlerService.GetAllPdfPaged(searchText, skip, take);
            return files;
        }
    }
}
