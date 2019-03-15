using Api.Helper.ContentWrapper.Core.ResponseModel;
using Api.Helper.ContentWrapper.Core.WrapperModel;
using Application.MongoDb.Core.Repository;
using Application.Core.Models; 
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.AppUser
{
    public class FileProcessedLogService : IFileProcessedLogService
    {
        private readonly IMongoRepository<FileProcessedLog, ObjectId> _repository; 
        public FileProcessedLogService(IMongoRepository<FileProcessedLog, ObjectId> repository)
        {
            _repository = repository; 
        } 
          
        public async Task<IEnumerable<FileProcessedLogDto>> GetAll()
        {
            try
            {
                var result = await _repository.GetAllListAsync();

                return result.Select(x => new FileProcessedLogDto
                {
                   Id = x.Id.ToString(),
                   action = x.action,
                   fileArchivedPath = x.fileArchivedPath,
                   filename = x.filename,
                   filePath = x.filePath,
                   remarks = x.remarks,
                   timestamp = x.timestamp
                }); 
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PagedResultDto<FileProcessedLogDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10)
        {
            try
            {
                var result = await _repository.GetAllListAsync();
                var res = result.Select(x => new FileProcessedLogDto
                {
                    Id = x.Id.ToString(),
                    action = x.action,
                    fileArchivedPath = x.fileArchivedPath,
                    filename = x.filename,
                    filePath = x.filePath,
                    remarks = x.remarks,
                    timestamp = x.timestamp
                });

                if (!string.IsNullOrEmpty(searchText))
                {
                    res = res.Where(x => x.action.ToLower().Contains(searchText)
                    || x.fileArchivedPath.ToLower().Contains(searchText)
                    || x.filename.ToLower().Contains(searchText)
                    || x.remarks.ToLower().Contains(searchText)
                    || x.filePath.ToLower().Contains(searchText));
                }  
                return new PagedResultDto<FileProcessedLogDto>(res.Count(), res.Skip(skip).Take(maxResultCount).ToList());
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<FileProcessedLogDto> Get(string id)
        {
            var result = await _repository.GetAsync(new ObjectId(id));
            if (result == null)
            {
                throw new ApiException("Your request is not valid", 500);
            }

            return new FileProcessedLogDto
            {

                Id = result.Id.ToString(),
                action = result.action,
                fileArchivedPath = result.fileArchivedPath,
                filename = result.filename,
                filePath = result.filePath,
                remarks = result.remarks,
                timestamp = result.timestamp
            };
        }

        public async Task<FileProcessedLogDto> Save(FileProcessedLogDto input)
        { 
            var saveModel = new FileProcessedLog 
            {
                action = input.action,
                fileArchivedPath = input.fileArchivedPath,
                filename = input.filename,
                filePath = input.filePath,
                remarks = input.remarks,
                timestamp = DateTime.Now
            }; 
             
            await _repository.InsertAsync(saveModel);
            return input;
        }

        public async Task<FileProcessedLogDto> Update(FileProcessedLogDto input)
        {  
            var saveModel = new FileProcessedLog
            {
                Id = new ObjectId(input.Id),
                action = input.action,
                fileArchivedPath = input.fileArchivedPath,
                filename = input.filename,
                filePath = input.filePath,
                remarks = input.remarks,
                timestamp = DateTime.Now
            };

            await _repository.InsertAsync(saveModel);
            return input;
        } 

        public async Task<bool> Delete(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
            return true;
        } 
    }
}
