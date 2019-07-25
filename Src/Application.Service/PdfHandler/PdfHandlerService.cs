using Application.Core.Dto.File;
using Application.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MongoDb.Repository;
using Api.ResultWrapper.AspNetCore.ResponseModel;

namespace Application.Service.PdfHandler
{
    public class PdfHandlerService : IPdfHandlerService
    {
        private readonly IOptions<Settings> _settings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMongoRepository<FileDirectorySetting, ObjectId> _repository;

        public PdfHandlerService(IOptions<Settings> settings, IHostingEnvironment hostingEnvironment, IMongoRepository<FileDirectorySetting, ObjectId> repository)
        {
            _settings = settings;
            _hostingEnvironment = hostingEnvironment;
            _repository = repository;
        }

        public async Task<ListResultDto<FileInfoDto>> GetAllPdf()
        {
            var result = new List<FileInfoDto>();
            try
            {
                var directoryPath = string.Empty;
                if (_settings.Value.UseFromDBConfig)
                {
                    var configFromDb = _repository.GetAll();
                    if (configFromDb.Any())
                    {
                        directoryPath = configFromDb.First().DirectoryPath;
                    }
                    else
                    {
                        directoryPath = _settings.Value.DirectoryPath;
                        // to get the result without breaking application

                        //this is the first time running up so create it
                        var configModel = new FileDirectorySetting
                        {
                            DirectoryPath = _settings.Value.DirectoryPath,
                            ArchiveDirectoryPath = _settings.Value.ArchiveDirectoryPath,
                            DefaultDirectoryPath = _settings.Value.DefaultDirectoryPath
                        }; 
                        await _repository.InsertAsync(configModel);
                    }
                }
                else
                {
                    directoryPath = _settings.Value.DirectoryPath; //get form config
                } 

                string webRootPath = _hostingEnvironment.WebRootPath; 
                var filesPath = webRootPath + "\\" + directoryPath;

                //var allFiles = Directory.EnumerateFiles(filesPath, "*", SearchOption.AllDirectories).ToList();
                 
                // Change the directory. In this case, first check to see
                // whether it already exists, and create it if it does not.
                // If this is not appropriate for your application, you can
                // handle the System.IO.IOException that will be raised if the
                // directory cannot be found.
                if (!System.IO.Directory.Exists(filesPath))
                {
                    System.IO.Directory.CreateDirectory(filesPath);
                } 
                System.IO.Directory.SetCurrentDirectory(filesPath);

                // Get the root directory and print out some information about it.
                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(filesPath);

                // Get the files in the directory and print out some information about them.
                System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.pdf");

                result = fileNames.OrderByDescending(x => x.CreationTime).Select(file => new FileInfoDto
                {
                    FileName = file.Name,
                    FilePath = "/" + _settings.Value.DirectoryPath + "/" + file.Name,
                    FileDescription = file.Name,
                    LastModifiedDate = file.LastAccessTime,
                    CreatedAt = file.CreationTime,
                    Size = ((file.Length) / (1024 * 1024)).ToString() + " MB"
                }).ToList();
                
                return new ListResultDto<FileInfoDto>(result);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PagedResultDto<FileInfoDto>> GetAllPdfPaged(string searchText, int skip, int take)
        {
            var result = new List<FileInfoDto>();
            try
            {
                var directoryPath = string.Empty;
                if (_settings.Value.UseFromDBConfig)
                {
                    var configFromDb = _repository.GetAll();
                    if (configFromDb.Any())
                    {
                        directoryPath = configFromDb.First().DirectoryPath;
                    }
                    else
                    {
                        directoryPath = _settings.Value.DirectoryPath;
                        // to get the result without breaking application

                        //this is the first time running up so create it
                        var configModel = new FileDirectorySetting
                        {
                            DirectoryPath = _settings.Value.DirectoryPath,
                            ArchiveDirectoryPath = _settings.Value.ArchiveDirectoryPath,
                            DefaultDirectoryPath = _settings.Value.DefaultDirectoryPath
                        };
                        await _repository.InsertAsync(configModel);
                    }
                }
                else
                {
                    directoryPath = _settings.Value.DirectoryPath; //get form config
                }
                string webRootPath = _hostingEnvironment.WebRootPath;

                var filesPath = webRootPath + "\\" + directoryPath;

                //var allFiles = Directory.EnumerateFiles(filesPath, "*", SearchOption.AllDirectories).ToList();

                // Change the directory. In this case, first check to see
                // whether it already exists, and create it if it does not.
                // If this is not appropriate for your application, you can
                // handle the System.IO.IOException that will be raised if the
                // directory cannot be found.
                if (!System.IO.Directory.Exists(filesPath))
                {
                    System.IO.Directory.CreateDirectory(filesPath);
                }
                System.IO.Directory.SetCurrentDirectory(filesPath);

                // Get the root directory and print out some information about it.
                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(filesPath);

                // Get the files in the directory and print out some information about them.
                System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.pdf");

                var fn = fileNames.ToList();

                if (!string.IsNullOrEmpty(searchText))
                {
                    fn = fn.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList();
                }

                var totalCount = fn.Count();
                 
                fn = fn.OrderByDescending(x => x.CreationTime).Skip(skip).Take(take).ToList();

                result = fn.Select(file => new FileInfoDto
                {
                    FileName = file.Name,
                    FilePath = "/" + _settings.Value.DirectoryPath + "/" + file.Name,
                    FileDescription = file.Name,
                    LastModifiedDate = file.LastAccessTime,
                    CreatedAt = file.CreationTime,
                    Size = ((file.Length) / (1024 * 1024)).ToString() + " MB"
                }).ToList();   
               
                return new PagedResultDto<FileInfoDto>(totalCount, result);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
